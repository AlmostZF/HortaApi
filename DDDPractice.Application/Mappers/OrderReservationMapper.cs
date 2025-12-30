using DDDPractice.DDDPractice.Domain;
using DDDPractice.DDDPractice.Domain.Entities;
using DDDPractice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.Application.Mappers;

public class OrderReservationMapper
{
    public static OrderReservationResponseDto ToDto(OrderReservationEntity orderReservationEntity)
    {
        
        return new OrderReservationResponseDto
        {
            Id = orderReservationEntity.Id,
            OrderStatus = orderReservationEntity.OrderStatus,
            PickupDate = orderReservationEntity.PickupDate,
            PickupDeadline = orderReservationEntity.PickupDeadline,
            PickupLocation = new PickupLocation(
                street: orderReservationEntity.PickupLocation.Street,
                city: orderReservationEntity.PickupLocation.City,
                state: orderReservationEntity.PickupLocation.State,
                number: orderReservationEntity.PickupLocation.Number
            ),
            ReservationDate = orderReservationEntity.ReservationDate,
            ReservationFee = orderReservationEntity.ReservationFee,
            ValueTotal = orderReservationEntity.ValueTotal,
            UserResponse = CustomerMapper.ToDto(orderReservationEntity.Customer),
            listOrderItens = OrderReservationItemMapper.ToDtoList(orderReservationEntity.ListOrderItems)
        };

    }

    public static List<OrderReservationResponseDto> ToDtoList(IEnumerable<OrderReservationEntity> orderReservationEntity)
    {
        return orderReservationEntity.Select(ToDto).ToList();
    }

    public static OrderCalculateResponseDto ToCalculatedOrderDTO(ICollection<OrderReservationItemEntity> itensCollection, decimal fee, decimal totalValue)
    {
        
        var orderCalculateResponse = new OrderCalculateResponseDto();
        orderCalculateResponse.Fee = fee;
        orderCalculateResponse.Total = totalValue;

        var listItens = itensCollection.Select(itemDto => new OrderReservationItemResponseDto()
            {
                ProductId = itemDto.ProductId,
                Id = null,
                Name = itemDto.Product.Name,
                Quantity = itemDto.Quantity,
                ReservationId = null,
                SellerId = itemDto.SellerId,
                SellerName = itemDto.Seller.Name,
                TotalPrice = itemDto.TotalPrice,
                UnitPrice = itemDto.UnitPrice,
                Image = itemDto.Product.Image
            })
            .ToList();

        orderCalculateResponse.ListOrderItens = listItens;
        return orderCalculateResponse;

    }


    public static OrderReservationEntity ToEntity(OrderReservationResponseDto orderReservationResponseDto)
    {
        var order =  new OrderReservationEntity
        {
            Id = Guid.NewGuid(),
            OrderStatus = orderReservationResponseDto.OrderStatus,
            PickupDate = orderReservationResponseDto.PickupDate,
            PickupDeadline = orderReservationResponseDto.PickupDeadline,
            PickupLocation =
            {
                Street = orderReservationResponseDto.PickupLocation.Street,
                City = orderReservationResponseDto.PickupLocation.City,
                State = orderReservationResponseDto.PickupLocation.State,
                Number = orderReservationResponseDto.PickupLocation.Number,
            },
            ReservationDate = orderReservationResponseDto.ReservationDate ?? DateTime.Now,
            ReservationFee = orderReservationResponseDto.ReservationFee,
            ValueTotal = orderReservationResponseDto.ValueTotal
        };
        foreach (var itemDto in orderReservationResponseDto.listOrderItens)
        {
            var item = new OrderReservationItemEntity()
            {
                Id = Guid.NewGuid(),
                ProductId = itemDto.ProductId,
                SellerId = itemDto.SellerId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                TotalPrice = itemDto.TotalPrice,
                ReservationId = order.Id
            };

            order.ListOrderItems.Add(item);
        }

        return order;
    }
    
    public static void ToUpdateEntity(OrderReservationEntity orderEntity, OrderReservationUpdateDTO orderReservationUpdateDto,
        ICollection<OrderReservationItemEntity> listOrderItem,  decimal reservationFee, decimal total)
    {
        orderEntity.OrderStatus = orderReservationUpdateDto.OrderStatus;
        orderEntity.ListOrderItems = listOrderItem;
        orderEntity.PickupDate = orderReservationUpdateDto.PickupDate;
        orderEntity.PickupDeadline = orderReservationUpdateDto.PickupDeadline;
        orderEntity.ReservationDate = orderReservationUpdateDto.ReservationDate;
        orderEntity.ReservationFee = reservationFee;
        orderEntity.SecurityCode = orderReservationUpdateDto.SecurityCode;
        orderEntity.ValueTotal = total;

    }
    public static OrderReservationEntity ToCreateEntity(OrderReservationCreateDTO orderReservationCreateDto, decimal reservationFee, decimal total)
    {
        try
        {
            var id = Guid.NewGuid();
            
            return new OrderReservationEntity()
            {
                Id = id,
                OrderStatus = orderReservationCreateDto.OrderStatus,
                ListOrderItems = [],
                PickupDate = orderReservationCreateDto.PickupDate,
                PickupDeadline = orderReservationCreateDto.PickupDeadline,
                PickupLocation = new PickupLocation(
                    street: orderReservationCreateDto.PickupLocation.Street,
                    city: orderReservationCreateDto.PickupLocation.City,
                    state: orderReservationCreateDto.PickupLocation.State,
                    number: orderReservationCreateDto.PickupLocation.Number
                ),
                ReservationDate = orderReservationCreateDto.ReservationDate,
                ReservationFee = reservationFee,
                SecurityCode = orderReservationCreateDto.SecurityCode,
                UserId = orderReservationCreateDto.UserId,
                ValueTotal = total
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
    public static List<OrderReservationEntity> ToEntitylist(List<OrderReservationResponseDto> orderReservationDto)
    {
        return orderReservationDto.Select(ToEntity).ToList();
    }

}