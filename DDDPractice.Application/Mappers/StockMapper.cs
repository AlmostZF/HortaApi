using DDDPractice.DDDPractice.Domain.Entities;
using DDDPractice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.Application.Mappers;

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


    public static StockEntity ToEntity(StockResponseDto stockResponseDto)
    {
        
        return new StockEntity
        {
            Id = Guid.NewGuid(),
            Quantity = stockResponseDto.Quantity,
            MovementDate = stockResponseDto.MovementDate ?? DateTime.Now,
            ProductId = stockResponseDto.ProductId,
        };

    }
    
    public static StockEntity ToCreateEntity(StockCreateDTO stockCreateDTO, decimal produtUnitPrice)
    {
        return new StockEntity
        {
            Id = Guid.NewGuid(),
            Quantity = stockCreateDTO.Quantity,
            MovementDate = DateTime.Now,
            ProductId = stockCreateDTO.ProductId,
            Total = StockMoney.CalculateTotal(produtUnitPrice, stockCreateDTO.Quantity).Amount,
            
        };

    }
    
    public static void ToUpdateEntity(StockEntity stockEntity, StockUpdateDTO stockUpdateDTO)
    {

        {
            stockEntity.Quantity = stockUpdateDTO.Quantity; 
            stockEntity.MovementDate = DateTime.Now;
            stockEntity.Total = StockMoney.CalculateTotal(stockEntity.Product.UnitPrice, stockUpdateDTO.Quantity).Amount;
        }

    }
    
    public static List<StockEntity> ToEntitylist(List<StockResponseDto> stockDto)
    {
        return stockDto.Select(ToEntity).ToList();
    }

}