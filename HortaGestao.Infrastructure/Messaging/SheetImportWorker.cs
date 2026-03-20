using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.UseCases.Product;
using HortaGestao.Application.UseCases.Stock;
using Microsoft.AspNetCore.Http;
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

        Console.WriteLine("Waiting for messages");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            using var scope = _scopeFactory.CreateScope();
    
            var createProductUseCase = scope.ServiceProvider.GetRequiredService<CreateProductUseCase>();
            var createStockUseCase = scope.ServiceProvider.GetRequiredService<CreateStockUseCase>();
            
            try
            {

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                using JsonDocument doc = JsonDocument.Parse(message);
                JsonElement root = doc.RootElement;
            
                var sellerId = Guid.Parse(root.GetProperty("userId").GetString());
                var productInfo = root.GetProperty("data");
                string base64Image = productInfo.GetProperty("Image").GetString();
                
                IFormFile imageFile = null;
                
                if (!string.IsNullOrEmpty(base64Image))
                {
                    byte[] imageBytes = Convert.FromBase64String(base64Image);
                    var stream = new MemoryStream(imageBytes);
                    imageFile = new FormFile(stream, 0, stream.Length, "Image", "produto.png")
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = "image/png"
                    };
                }
            
                var productDto = new ProductCreateDto
                {
                    Name = productInfo.GetProperty("Name").GetString(),
                    ProductType = productInfo.GetProperty("ProductType").GetString(),
                    LargeDescription = productInfo.GetProperty("LargeDescription").GetString(),
                    ShortDescription = productInfo.GetProperty("ShortDescription").GetString(),
                    UnitPrice = productInfo.GetProperty("UnitPrice").ValueKind == JsonValueKind.Null 
                        ? 0 : productInfo.GetProperty("UnitPrice").GetDecimal(),
                    Weight = productInfo.GetProperty("Weight").GetString(),
                    ConservationDays = productInfo.GetProperty("ConservationDays").GetString(),
                    Image = imageFile
                };
            
                Console.WriteLine($"[TESTE] Processando produto: {productDto.Name} para o user: {sellerId}");
            
                var productId = await createProductUseCase.ExecuteAsync(productDto, sellerId);

                var stockCreateDto = new StockCreateDto
                {
                    ProductId = productId.Value,
                    Quantity = productInfo.GetProperty("Quantity").ValueKind == JsonValueKind.Null 
                        ? 0 : productInfo.GetProperty("Quantity").GetInt32(),
                };
            
                await createStockUseCase.ExecuteAsync(stockCreateDto, sellerId);

                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        };

        await channel.BasicConsumeAsync("import_sheet", autoAck: false, consumer: consumer);
        
        await Task.Delay(Timeout.Infinite, stoppingToken);

    }
}
