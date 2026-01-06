using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Application.Mappers;

public class StockMapper
{
    public static StockResponseDto ToDto(StockEntity stockEntity)
    {
        return new StockResponseDto
        {
            Id = stockEntity.Id,
            Product = ProductMapper.ToDto(stockEntity.Product),
            Quantity = stockEntity.Quantity,
            MovementDate = stockEntity.MovementDate,
            ProductId = stockEntity.ProductId,
            Total = StockMoney.CalculateTotal(stockEntity.Product.UnitPrice, stockEntity.Quantity).Amount,
        };

    }
    public static StockAvailableResponseDto ToAvailableDto(StockEntity stockEntity)
    {
        return new StockAvailableResponseDto
        {
            Product = ProductMapper.ToDto(stockEntity.Product),
            StockLimit = stockEntity.Quantity,
        };

    }

    public static List<StockResponseDto> ToDtoList(IEnumerable<StockEntity> stockEntity)
    {
        return stockEntity.Select(ToDto).ToList();
    }
    
    
    public static StockEntity ToCreateEntity(StockCreateDto stockCreateDTO, decimal produtUnitPrice)
    {
        return new StockEntity(stockCreateDTO.ProductId, stockCreateDTO.Quantity);

    }
    
    public static void ToUpdateEntity(StockEntity stockEntity, StockUpdateDto stockUpdateDTO)
    {
        
        stockEntity.UpdateStock(stockEntity.ProductId, stockEntity.Product.UnitPrice, stockUpdateDTO.Quantity);

    }
    

}