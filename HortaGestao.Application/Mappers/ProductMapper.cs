using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Request.ProductCreateDTO;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Application.Mappers;

public class ProductMapper
{
    public static ProductResponseDto ToDto(ProductEntity productEntity)
    {
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


    public static ProductEntity ToEntity(ProductCreateDto productCreateDto)
    {
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
    
    public static ProductEntity ToCreateEntity(ProductCreateDto productCreateDto)
    {
    
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
    
    public static void ToUpdateEntity(ProductEntity productEntity, ProductUpdateDto productUpdateDto)
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

    public static ProductFilter toFilter(ProductFilterDto productFilterDto)
    {
        return new ProductFilter(
            productFilterDto.Category,
            productFilterDto.Name,
            productFilterDto.Seller,
            productFilterDto.PageNumber,
            productFilterDto.MaxItensPerPage);
    }

    public static List<ProductEntity> ToEntitylist(List<ProductCreateDto> productDto)
    {
        return productDto.Select(ToEntity).ToList();
    }



}