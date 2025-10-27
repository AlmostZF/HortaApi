using DDD_Practice.DDDPractice.Domain.Entities;
using DDD_Practice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.Application.Mappers;

public class ProductMapper
{
    public static ProductResponseDto ToDto(ProductEntity productEntity)
    {
        if (productEntity == null) return new ProductResponseDto();
        
        
        return new ProductResponseDto()
        {
            Id = productEntity.Id,
            Name = productEntity.Name,
            ProductType = productEntity.ProductType, 
            UnitPrice = productEntity.UnitPrice,
            Seller = SellerMapper.ToDto(productEntity.Seller),
            ConservationDays = productEntity.ConservationDays,
            Image = productEntity.Image,
            LargeDescription = productEntity.LargeDescription,
            ShortDescription = productEntity.ShortDescription,
            Weight = productEntity.Weight
        };

    }

    public static List<ProductResponseDto> ToDtoList(IEnumerable<ProductEntity> productEntity)
    {
        return productEntity.Select(ToDto).ToList();
    }


    public static ProductEntity ToEntity(ProductCreateDTO productCreateDto)
    {
        if (productCreateDto == null) return new ProductEntity();
        
        return new ProductEntity
        {
            Id = Guid.NewGuid(),
            Name = productCreateDto.Name,
            ProductType = productCreateDto.ProductType,
            UnitPrice = productCreateDto.UnitPrice,
            SellerId = productCreateDto.SellerId,
            ConservationDays = productCreateDto.ConservationDays,
            Image = productCreateDto.Image,
            LargeDescription = productCreateDto.LargeDescription,
            ShortDescription = productCreateDto.ShortDescription,
            Weight = productCreateDto.Weight,
        };
    }
    
    public static ProductEntity ToCreateEntity(ProductCreateDTO productCreateDto)
    {
        if (productCreateDto == null) return new ProductEntity();
    
        return new ProductEntity
        {
            Id = Guid.NewGuid(),
            Name = productCreateDto.Name,
            ProductType = productCreateDto.ProductType,
            UnitPrice = productCreateDto.UnitPrice,
            SellerId = productCreateDto.SellerId,
            ConservationDays = productCreateDto.ConservationDays,
            Image = productCreateDto.Image,
            LargeDescription = productCreateDto.LargeDescription,
            ShortDescription = productCreateDto.ShortDescription,
            Weight = productCreateDto.Weight
        };
    }
    
    public static void ToUpdateEntity(ProductEntity productEntity, ProductUpdateDTO productUpdateDto)
    {
        {
            productEntity.Id = productUpdateDto.Id;
            productEntity.Name = productUpdateDto.Name;
            productEntity.ProductType = productUpdateDto.ProductType;
            productEntity.UnitPrice = productUpdateDto.UnitPrice;
            productEntity.SellerId = productUpdateDto.SellerId;
            productEntity.ConservationDays = productUpdateDto.ConservationDays;
            productEntity.Image = productUpdateDto.Image;
            productEntity.LargeDescription = productUpdateDto.LargeDescription;
            productEntity.ShortDescription = productUpdateDto.ShortDescription;
            productEntity.Weight = productUpdateDto.Weight;
        };
    }

    public static ProductFilter toFilter(ProductFilterDTO productFilterDto)
    {
        if(productFilterDto == null) return new ProductFilter(null, null, null,null, null);

        return new ProductFilter(
            productFilterDto.Category,
            productFilterDto.Name,
            productFilterDto.Seller,
            productFilterDto.PageNumber,
            productFilterDto.MaxItensPerPage);
    }

    public static List<ProductEntity> ToEntitylist(List<ProductCreateDTO> productDto)
    {
        return productDto.Select(ToEntity).ToList();
    }



}