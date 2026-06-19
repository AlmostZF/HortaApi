
using System.Text;
using System.Text.Json;
using HortaGestao.Application.DTOs.Request;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;

namespace HortaGestao.Infrastructure.Messaging;

public class SheetImportWorker : BackgroundService
{

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<ImportHub> _hubContext;

    public SheetImportWorker(IServiceScopeFactory scopeFactory, IHubContext<ImportHub> hubContext)
    {
        _scopeFactory = scopeFactory;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync(stoppingToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        
        await channel.QueueDeclareAsync(queue: "import_sheet", durable: false, exclusive: false, autoDelete: false,
            arguments: null);
        
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 5, global: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        
        consumer.ReceivedAsync += async (model, ea) =>
        {
            _ = Task.Run(async () =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var importData = JsonSerializer.Deserialize<ImportMessagingDto>(message);
                
                if (importData == null || importData.Products == null)
                {
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                    return;
                }

                using (var scope = _scopeFactory.CreateScope())
                {
                    try
                    {
                        var processor = scope.ServiceProvider.GetRequiredService<ISheetImportProcessor>();
                        await processor.ProcessBatchAsync(importData, _hubContext);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Erro crítico no processamento da planilha: {e.Message}");
                        
                    }
                    finally
                    {
                        await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                }
            },stoppingToken);
            await Task.CompletedTask;
        };
            await channel.BasicConsumeAsync("import_sheet", autoAck: false, consumer: consumer);
            await Task.Delay(Timeout.Infinite, stoppingToken);
    }

}
