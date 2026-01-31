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

    
    public static ProductEntity ToCreateEntity(ProductCreateDto productCreateDto, string imagePath, Guid sellerId)
    {
        var productType = ProductEntity.StringToProductType(productCreateDto.ProductType); 
        return new ProductEntity( productCreateDto.Name, productType, productCreateDto.UnitPrice,
            sellerId, productCreateDto.ConservationDays, imagePath,
            productCreateDto.ShortDescription,
            productCreateDto.LargeDescription, productCreateDto.Weight);
    }
    
    public static void ToUpdateEntity(ProductEntity productEntity, ProductUpdateDto productUpdateDto, string imagePath ,Guid sellerId)
    {
        productEntity.UpdateProductEntity(
            productUpdateDto.Name,
            productUpdateDto.ProductType,
            productUpdateDto.UnitPrice,
            sellerId,
            productUpdateDto.ConservationDays,
            imagePath,
            productUpdateDto.LargeDescription,
            productUpdateDto.ShortDescription,
            productUpdateDto.Weight,
            productUpdateDto.IsActive);

    }

    public static void ToUpdateStatus(ProductEntity productEntity, bool status)
    {
        if (status) productEntity.Activate();
        else productEntity.Deactivate();
    }

    public static ProductFilter toFilter(ProductFilterDto productFilterDto)
    {
        return new ProductFilter(
            productFilterDto.Category,
            productFilterDto.Name,
            productFilterDto.Seller,
            productFilterDto.PageNumber,
            productFilterDto.MaxItensPerPage,
            productFilterDto.OrderByLowestPrice);
    }
    



}