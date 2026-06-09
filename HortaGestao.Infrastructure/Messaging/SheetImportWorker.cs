using System.Text;
using System.Text.Json;
using HortaGestao.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
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
    private int _processedItems = 0; 
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
        
        int processedItems = 0;
        

        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
        
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            var importData = JsonSerializer.Deserialize<ImportMessagingDto>(message);
            
            string planilhaId = importData.ImportId.ToString(); 
            int total = importData.TotalMessages;
            int current = Interlocked.Increment(ref _processedItems);
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var processor = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ISheetImportProcessor>();
                    var messageDto = await processor.ProcessMessageAsync(message);
                    double percentage = total > 0 ? (double)current / total * 100 : 0;

                    if (double.IsInfinity(percentage) || double.IsNaN(percentage)) 
                    {
                        percentage = 0;
                    }
                    
                    if (current >= total)
                    {
                        Interlocked.Exchange(ref _processedItems, 0);
                    }
                    
                    await _hubContext.Clients.All.SendAsync("ReceiveProgress", new {
                        Current = current,
                        Total = total,
                        Percentage = Math.Round(percentage, 2),
                        MessageDto = messageDto
                    });
                    
                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception e)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveImportError", new {
                        ErrorMessage = e.Message,
                        Data = importData.Product.Name
                    });
                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                    throw;
                }
            }
        };

        await channel.BasicConsumeAsync("import_sheet", autoAck: false, consumer: consumer);
        
        await Task.Delay(Timeout.Infinite, stoppingToken);
        
    }
}

[Authorize]
public class ImportHub : Hub
{
}
