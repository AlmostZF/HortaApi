using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
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
            ProductType = productEntity.ProductType.ToString(), 
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

    
    public static ProductEntity ToCreateEntity(ProductCreateDto productCreateDto)
    {
        
        return new ProductEntity( productCreateDto.Name, productCreateDto.ProductType, productCreateDto.UnitPrice,
            productCreateDto.SellerId, productCreateDto.ConservationDays, productCreateDto.Image,
            productCreateDto.ShortDescription,
            productCreateDto.LargeDescription, productCreateDto.Weight);
    }
    
    public static void ToUpdateEntity(ProductEntity productEntity, ProductUpdateDto productUpdateDto)
    {
        productEntity.UpdateProductEntity(
            productUpdateDto.Name,
            productUpdateDto.ProductType,
            productUpdateDto.UnitPrice,
            productUpdateDto.SellerId,
            productUpdateDto.ConservationDays,
            productUpdateDto.Image,
            productUpdateDto.LargeDescription,
            productUpdateDto.ShortDescription,
            productUpdateDto.Weight);

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
    



}