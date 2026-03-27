using System.Text.Json;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.UseCases.MessagingLog;
using HortaGestao.Application.UseCases.Product;
using HortaGestao.Application.UseCases.Stock;
using HortaGestao.Infrastructure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HortaGestao.Infrastructure.Interfaces;

public class SheetImportProcessor: ISheetImportProcessor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ErrorQueuePublisher _errorWorker;

    public SheetImportProcessor(IServiceProvider serviceProvider, ErrorQueuePublisher errorWorker )
    {
        _serviceProvider = serviceProvider;
        _errorWorker = errorWorker;
    }

    public async Task ProcessMessageAsync(string messageContent)
    {
        using var scope = _serviceProvider.CreateScope();
    
        var createProductUseCase = scope.ServiceProvider.GetRequiredService<CreateProductUseCase>();
        var createStockUseCase = scope.ServiceProvider.GetRequiredService<CreateStockUseCase>();
        var createMesagingLog = scope.ServiceProvider.GetRequiredService<CreateMessagingLogUseCase>();
        ImportMessagingDto importData = null;
        
        try
        {
            importData = await ParseMessage(messageContent);
            var product = await createProductUseCase.ExecuteAsync(importData.Product, importData.SellerId);

            if (product.IsSuccess == false)
            {
                var result = await createMesagingLog.ExecuteAsync(importData, product.Error);
                await _errorWorker.PublishErrorAsync(result.Value);
                return;
            }

            var stockDto = new StockCreateDto { ProductId = product.Value, Quantity = importData.Quantity };
            await createStockUseCase.ExecuteAsync(stockDto, importData.SellerId);

        }
        catch (Exception e)
        {
            if (importData == null) 
            {
                await _errorWorker.PublishErrorAsync(new MessagingLogDto {
                    MessageError = $"Erro crítico no mapeamento do JSON: {e.Message}",
                    CreatedAt = DateTime.Now
                });
            }
            else 
            {
                var result = await createMesagingLog.ExecuteAsync(importData, e.Message);
                await _errorWorker.PublishErrorAsync(result.Value);
            }
        
            throw;
        }


    }
    
    private async Task<ImportMessagingDto> ParseMessage(string json)
    {
        using JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;
        
        var productInfo = root.GetProperty("Data");
        string base64Image = productInfo.GetProperty("Image").GetString();

        var image = createImage(base64Image);
        var product = createDto(productInfo, image);
        
        
        var quantity = productInfo.GetProperty("Quantity").ValueKind == JsonValueKind.Null
            ? 0
            : productInfo.GetProperty("Quantity").GetInt32();
        
        var line = productInfo.GetProperty("Line").GetString();
        var idStr = root.GetProperty("ImportId").GetString();
        var userStr = root.GetProperty("UserId").GetString();

        var result = new ImportMessagingDto
        {
            ImportId = Guid.Parse(idStr),
            SellerId = Guid.Parse(userStr),
            Product = product,
            Timestamp = root.GetProperty("Timestamp").GetString(),
            Quantity = quantity,
            Line = line
        };

        return result;
    }

    private IFormFile createImage(string base64Image)
    {
        IFormFile imageFile = null;
                
        if (!string.IsNullOrEmpty(base64Image))
        {
            var base64Data = base64Image.Contains(",") ? base64Image.Split(',')[1] : base64Image;
            byte[] imageBytes = Convert.FromBase64String(base64Data);
            var stream = new MemoryStream(imageBytes);
            stream.Position = 0;
            
            imageFile = new FormFile(stream, 0, stream.Length, "Image", "produto.png")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };
        }

        return imageFile;
    }

    private ProductCreateDto createDto(JsonElement productInfo, IFormFile imageFile)
    {
        return new ProductCreateDto
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
    }
    
}
