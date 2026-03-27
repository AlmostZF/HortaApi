using System.Text;
using System.Text.Json;
using HortaGestao.Application.DTOs.Response;
using RabbitMQ.Client;

namespace HortaGestao.Infrastructure.Messaging;

public class ErrorQueuePublisher
{
    public async Task PublishErrorAsync(MessagingLogDto message)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        
        await channel.QueueDeclareAsync(queue: "Error_Sheet_Import", durable: true, exclusive: false, autoDelete: false,
            arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } });
        
        var messageJson = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageJson);

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "Error_Sheet_Import", body: body);
        Console.WriteLine($" [x] Sent {message}");

    }
}