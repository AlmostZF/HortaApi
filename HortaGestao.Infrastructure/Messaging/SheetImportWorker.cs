using System.Text;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;

namespace HortaGestao.Infrastructure.Messaging;

public class SheetImportWorker: BackgroundService
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

        
        var messageCount = await channel.MessageCountAsync("import_sheet");
        int total = (int)messageCount;
        int processedItems = 0;
        
        if (total == 0) return;
        
        await channel.QueueDeclareAsync(queue: "import_sheet", durable: false, exclusive: false, autoDelete: false,
            arguments: null);

        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
        
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            try
            {
                
                var processor = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ISheetImportProcessor>();
                await processor.ProcessMessageAsync(message);

                int current = Interlocked.Increment(ref processedItems);
                double percentage = (double)processedItems / total * 100;
                
                await _hubContext.Clients.All.SendAsync("ReceiveProgress", new {
                    Current = current,
                    Total = total,
                    Percentage = percentage
                });
                
                Console.WriteLine($"Progresso: {percentage:F2}% ({current}/{total})");
                
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao processar: {message}");
                Console.WriteLine($"Erro ao processar: {e}");
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                throw;
            }

        };

        await channel.BasicConsumeAsync("import_sheet", autoAck: false, consumer: consumer);
        
        await Task.Delay(Timeout.Infinite, stoppingToken);

    }
}

public class ImportHub : Hub
{
}
