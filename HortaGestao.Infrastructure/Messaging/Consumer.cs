using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace HortaGestao.Infrastructure.Messaging;

public class Consumer: BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "import_sheet", durable: false, exclusive: false, autoDelete: false,
            arguments: null);

        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

        Console.WriteLine("Waiting for messages");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            Console.WriteLine($" [x] Received {message}");
            Console.WriteLine($" [x] Received {message}");

            int dots = message.Split('.').Length - 1;
            await Task.Delay(dots * 1000);
            Console.WriteLine(" [x] Done");

            await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        await channel.BasicConsumeAsync("import_sheet", autoAck: false, consumer: consumer);
        
        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
        
        while (!stoppingToken.IsCancellationRequested) 
        { 
            await Task.Delay(1000, stoppingToken); 
        }

    }
}
