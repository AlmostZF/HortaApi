using DDDPractice.DDDPractice.Domain;
using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Domain.Entities;

namespace HortaGestao.Application.Mappers;

public static class OrderReservationItemMapper
{
    public static OrderReservationItemResponseDto ToDto(OrderReservationItemEntity orderReservationItemEntity)
    {
        if (orderReservationItemEntity == null)  throw new ArgumentException("Order Item must have at least one item.");
        
        return new OrderReservationItemResponseDto
        {
            Id = Guid.NewGuid(),
            Quantity = orderReservationItemEntity.Quantity,
            ProductId = orderReservationItemEntity.ProductId,
            ReservationId = orderReservationItemEntity.ReservationId,
            SellerId = orderReservationItemEntity.SellerId,
            TotalPrice = orderReservationItemEntity.TotalPrice,
            UnitPrice = orderReservationItemEntity.UnitPrice,
            // Name = orderReservationItemEntity.Product.Name,
            // SellerName = orderReservationItemEntity.Seller.Name,
            // Image = orderReservationItemEntity.Product.Image
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
        return new OrderReservationItemEntity(
            orderReservationResponseDto.ReservationId,
            orderReservationResponseDto.ProductId,
            orderReservationResponseDto.SellerId,
            orderReservationResponseDto.Quantity,
            orderReservationResponseDto.UnitPrice
            ) {
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