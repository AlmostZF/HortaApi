using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;

namespace HortaGestao.Infrastructure.Messaging;

public class SheetImportWorker: BackgroundService
{
    
    private readonly IServiceScopeFactory _scopeFactory;
    
    public SheetImportWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync(stoppingToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

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
