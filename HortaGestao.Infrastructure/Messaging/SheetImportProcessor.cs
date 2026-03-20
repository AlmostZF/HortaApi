using System.Text.Json;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.UseCases.Product;
using HortaGestao.Application.UseCases.Stock;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HortaGestao.Infrastructure.Interfaces;

public class SheetImportProcessor: ISheetImportProcessor
{
    private readonly IServiceProvider _serviceProvider;

    public SheetImportProcessor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ProcessMessageAsync(string messageContent)
    {
        using var scope = _serviceProvider.CreateScope();
    
        var createProductUseCase = scope.ServiceProvider.GetRequiredService<CreateProductUseCase>();
        var createStockUseCase = scope.ServiceProvider.GetRequiredService<CreateStockUseCase>();
        
        try
        {
            var (productDto, sellerId, quantity) = ParseMessage(messageContent);
            var product = await createProductUseCase.ExecuteAsync(productDto, sellerId);

            if (product.IsSuccess == false)
                throw new Exception(product.Error);

            var stockDto = new StockCreateDto { ProductId = product.Value, Quantity = quantity };
            await createStockUseCase.ExecuteAsync(stockDto, sellerId);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


    }
    
    private (ProductCreateDto dto, Guid sellerId, int quantity) ParseMessage(string json)
    {
        using JsonDocument doc = JsonDocument.Parse(json);
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

        var quantity = productInfo.GetProperty("Quantity").ValueKind == JsonValueKind.Null
            ? 0
            : productInfo.GetProperty("Quantity").GetInt32();

        return (productDto, sellerId, quantity);
    }
}