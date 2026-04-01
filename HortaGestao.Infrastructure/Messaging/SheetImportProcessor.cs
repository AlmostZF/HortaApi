using System.Text.Json;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.UseCases.CreateProductWithStockUseCase;
using HortaGestao.Application.UseCases.MessagingLog;
using HortaGestao.Infrastructure.Messaging;
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

    public async Task<MessagingLogDto> ProcessMessageAsync(string messageContent)
    {
        using var scope = _serviceProvider.CreateScope();
        
        var createMesagingLog = scope.ServiceProvider.GetRequiredService<CreateMessagingLogUseCase>();
        var createProductWithStock = scope.ServiceProvider.GetRequiredService<CreateProductWithStockUseCase>();
        
        ImportMessagingDto importData = null;
        
        try
        {
            importData = await ParseMessage(messageContent);

            var product = await createProductWithStock.ExecuteAsync(importData.Product, importData.Quantity,
                importData.SellerId);
            
                var result = await createMesagingLog.ExecuteAsync(importData, product.Error);
                await _errorWorker.PublishErrorAsync(result.Value);
                return result.Value;

        }
        catch (Exception e)
        {
            if (importData == null)
            {
                await _errorWorker.PublishErrorAsync(new MessagingLogDto {
                    MessageError = $"Erro crítico no mapeamento do JSON: {e.Message}",
                    CreatedAt = DateTime.Now
                });
                
                return new MessagingLogDto
                {
                    MessageError = $"Erro crítico no mapeamento do JSON: {e.Message}",
                    CreatedAt = DateTime.Now
                };
            }
            
            var result = await createMesagingLog.ExecuteAsync(importData, e.Message);
            await _errorWorker.PublishErrorAsync(result.Value);
            return result.Value;
        }
        
    }
    
    private async Task<ImportMessagingDto> ParseMessage(string json)
    {
        using JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;
        
        var productInfo = root.GetProperty("Data");
        
        var product = createDto(productInfo);
        
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
    
    private ProductCreateDto createDto(JsonElement productInfo)
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
            Image = null
        };
    }
    
}
