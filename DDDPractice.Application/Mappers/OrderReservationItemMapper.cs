using DDD_Practice.DDDPractice.Domain;
using DDD_Practice.DDDPractice.Domain.Entities;
using DDDPractice.Application.DTOs;

namespace DDDPractice.Application.Mappers;

public static class OrderReservationItemMapper
{
    public static OrderReservationItemResponseDto ToDto(OrderReservationItemEntity orderReservationItemEntity)
    {
        if (orderReservationItemEntity == null) return new OrderReservationItemResponseDto();
        
        return new OrderReservationItemResponseDto
        {
            Id = Guid.NewGuid(),
            Quantity = orderReservationItemEntity.Quantity,
            ProductId = orderReservationItemEntity.ProductId,
            ReservationId = orderReservationItemEntity.ReservationId,
            SellerId = orderReservationItemEntity.SellerId,
            TotalPrice = orderReservationItemEntity.TotalPrice,
            UnitPrice = orderReservationItemEntity.UnitPrice,
            Name = orderReservationItemEntity.Product.Name,
            SellerName = orderReservationItemEntity.Seller.Name,
            Image = orderReservationItemEntity.Product.Image
            // Product = ProductMapper.ToDto(orderReservationItemEntity.Product),
            // Seller = SellerMapper.ToDto(orderReservationItemEntity.Seller)
        };

    }

    public static List<OrderReservationItemResponseDto> ToDtoList(IEnumerable<OrderReservationItemEntity> orderReservationItemEntity)
    {
        return orderReservationItemEntity.Select(ToDto).ToList();
    }


    public static OrderReservationItemEntity ToEntity(OrderReservationItemResponseDto orderReservationResponseDto)
    {
        return new OrderReservationItemEntity
        {
            Id = Guid.NewGuid(),
            Quantity = orderReservationResponseDto.Quantity,
            ProductId = orderReservationResponseDto.ProductId,
            ReservationId = orderReservationResponseDto.ReservationId.Value,
            SellerId = orderReservationResponseDto.SellerId,
            TotalPrice = (orderReservationResponseDto.Quantity * orderReservationResponseDto.UnitPrice),
            UnitPrice = orderReservationResponseDto.UnitPrice
        };
    }
    
    public static List<OrderReservationItemEntity> ToEntitylist(List<OrderReservationItemEntity> orderReservationDto)
    {
        return orderReservationDto.ToList();
    }
    public static List<OrderReservationItemEntity> ToEntitylist(List<OrderReservationItemResponseDto> orderReservationDto)
    {
        return orderReservationDto.Select(ToEntity).ToList();
    }
}