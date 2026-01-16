using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Application.Mappers;

public class OrderReservationMapper
{
    public static OrderReservationResponseDto ToDto(OrderReservationEntity orderReservationEntity)
    {
        if (orderReservationEntity is null)
            throw new ArgumentNullException(
                nameof(orderReservationEntity),
                "A entidade orderReservationEntity não pode ser nula para mapeamento."
            );
        
        return new OrderReservationResponseDto
        {
            Id = orderReservationEntity.Id,
            OrderStatus = orderReservationEntity.GetOrderString(),
            PickupDate = orderReservationEntity.PickupDate,
            PickupDeadline = orderReservationEntity.PickupDeadline,
            PickupLocation = PickupLocationMapper.ToDto(orderReservationEntity.PickupLocation),
            ReservationDate = orderReservationEntity.ReservationDate,
            ReservationFee = orderReservationEntity.ReservationFee,
            ValueTotal = orderReservationEntity.TotalValue,
            UserResponse = orderReservationEntity.Customer != null 
                ? CustomerMapper.ToDto(orderReservationEntity.Customer)
                : GuessCustomerMapper(orderReservationEntity),
            listOrderItens = OrderReservationItemMapper.ToDtoList(orderReservationEntity.ListOrderItems)
        };

    }

    private static CustomerResponseDto GuessCustomerMapper(OrderReservationEntity orderReservationEntity)
    {
        return new CustomerResponseDto()
        {
            Id = null,
            Name = orderReservationEntity.GuessCustomer.FullName,
            PhoneNumber = orderReservationEntity.GuessCustomer.PhoneNumber,
            SecurityCode = orderReservationEntity.Customer.SecurityCode.GenerateSecurityCode(),
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
                ReservationId = itemDto.ReservationId,
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
    
    public static void ToUpdateEntity(OrderReservationEntity orderEntity, OrderReservationUpdateDto orderReservationUpdateDto,
        ICollection<OrderReservationItemEntity> listOrderItem,  decimal reservationFee)
    {
        orderEntity.UpdateOrderStatus(MapStatus(orderReservationUpdateDto.OrderStatus));
        // orderEntity.SetItems(listOrderItem);
        // orderEntity.SchedulePickup(orderReservationUpdateDto.PickupDate, orderReservationUpdateDto.PickupDeadline);
        // orderEntity.SetReservationDate(orderReservationUpdateDto.ReservationDate);
        // orderEntity.ApplyFee(reservationFee);
        
    }
    public static OrderReservationEntity ToCreateEntity(OrderReservationCreateDto orderReservationCreateDto, decimal reservationFee, decimal total, Guid sellerId)
    {
        try
        {
            if (!String.IsNullOrEmpty(orderReservationCreateDto.FullName) && orderReservationCreateDto.UserId == null)
            {
                return OrderReservationEntity.CreateForGuest(
                    orderReservationCreateDto.FullName,
                    orderReservationCreateDto.Email,
                    orderReservationCreateDto.PhoneNumber,
                    PickupLocationMapper.ToEntity(orderReservationCreateDto.PickupLocation),
                    orderReservationCreateDto.PickupDate,
                    orderReservationCreateDto.PickupDeadline,
                    reservationFee,
                    sellerId);
            }
            
            return OrderReservationEntity.CreateForCustomer(
                orderReservationCreateDto!.UserId.Value,
                PickupLocationMapper.ToEntity(orderReservationCreateDto.PickupLocation),
                orderReservationCreateDto.PickupDate,
                orderReservationCreateDto.PickupDeadline,
                reservationFee,
                sellerId
            );
        


        }
        catch (Exception e)
        {
            throw new ArgumentException("Error to  create order reservation", e);
        }
        
    }
    
    // public static OrderReservationEntity ToEntity(OrderReservationCreateDto orderReservationCreateDto)
    // {
    //     if (orderReservationCreateDto == null)  throw new ArgumentException("Order must have at least one item.");;
    //     
    //     var order =  new OrderReservationEntity
    //     {
    //         Id = Guid.NewGuid(),
    //         OrderStatus = orderReservationResponseDto.OrderStatus,
    //         PickupDate = orderReservationResponseDto.PickupDate,
    //         PickupDeadline = orderReservationResponseDto.PickupDeadline,
    //         PickupLocation =
    //         {
    //             Street = orderReservationResponseDto.PickupLocation.Street,
    //             City = orderReservationResponseDto.PickupLocation.City,
    //             State = orderReservationResponseDto.PickupLocation.State,
    //             Number = orderReservationResponseDto.PickupLocation.Number,
    //         },
    //         ReservationDate = orderReservationResponseDto.ReservationDate ?? DateTime.Now,
    //         ReservationFee = orderReservationResponseDto.ReservationFee,
    //         ValueTotal = orderReservationResponseDto.ValueTotal
    //     };
    //     foreach (var itemDto in orderReservationResponseDto.listOrderItens)
    //     {
    //         var item = new OrderReservationItemEntity()
    //         {
    //             Id = Guid.NewGuid(),
    //             ProductId = itemDto.ProductId,
    //             SellerId = itemDto.SellerId,
    //             Quantity = itemDto.Quantity,
    //             UnitPrice = itemDto.UnitPrice,
    //             TotalPrice = itemDto.TotalPrice,
    //             ReservationId = order.Id
    //         };
    //
    //         order.ListOrderItems.Add(item);
    //     }
    //
    //     return order;
    // }
    
    // public static List<OrderReservationEntity> ToEntitylist(List<OrderReservationResponseDto> orderReservationDto)
    // {
    //     return orderReservationDto.Select(ToCreateEntity).ToList();
    // }
    
    private static StatusOrder MapStatus(string status)
    {
        return status.Trim().ToLower() switch
        {
            "pendente" => StatusOrder.Pendente,
            "confirmada" => StatusOrder.Confirmada,
            "expirada" => StatusOrder.Expirada,
            "cancelada" => StatusOrder.Cancelada,
            _ => throw new ArgumentException($"Status inválido: {status}")
        };
    }

}