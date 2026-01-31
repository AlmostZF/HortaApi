using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Domain.Entities;

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
            SecurityCode = orderReservationEntity.SecurityCode.Value == null
                ? orderReservationEntity.Customer.SecurityCode.GenerateSecurityCode()
                : orderReservationEntity.SecurityCode.Value,
        };
    }

    public static List<OrderReservationResponseDto> ToDtoList(IEnumerable<OrderReservationEntity> orderReservationEntity)
    {
        return orderReservationEntity.Select(ToDto).ToList();
    }

    public static OrderCalculateResponseDto ToCalculatedOrderDTO(ICollection<OrderReservationItemEntity> itensCollection,
        IEnumerable<ProductEntity> products, Dictionary<Guid, int> stock)
    {
    
        var listItens = itensCollection.Select(itemDto => 
        {
            stock.TryGetValue(itemDto.ProductId, out var maxQuantity);
            var productEntity = products.FirstOrDefault(p => p.Id == itemDto.ProductId);
            return new OrderReservationItemResponseDto()
            {
                ProductId = itemDto.ProductId,
                Name = productEntity?.Name ?? "Produto sem nome",
                Quantity = itemDto.Quantity,
                SellerId = itemDto.SellerId,
                SellerName = productEntity?.Seller?.Name ?? "Vendedor não informado",
                UnitPrice = itemDto.UnitPrice,
                TotalPrice = itemDto.Quantity * itemDto.UnitPrice,
                Image = productEntity?.Image,
                MaxQuantity = maxQuantity
            };

        }).ToList();
        
        var firstProductWithSeller = products.FirstOrDefault(p => p.Seller != null);
        
        SellerResponseDto? sellerDto = null;
        
        if(firstProductWithSeller != null)
        {
            var seller = firstProductWithSeller?.Seller;
            sellerDto = new SellerResponseDto()
            {
                Id = seller.Id,
                Name = seller.Name,
                PhoneNumber = seller.PhoneNumber,
                ListPickupLocations = seller.PickupLocations?.Select(loc => new PickupLocationResponseDto
                {
                    Id = loc.Id,
                    Name = "Endereço ",
                    Street = loc.Address.Street,
                    Number = loc.Address.Number,
                    City = loc.Address.City,
                    State = loc.Address.State,
                    ZipCode = loc.Address.ZipCode,
                    Neighborhood = loc.Address.Neighborhood,
                    PickupDays = loc.AvailablePickupDays.Select(day => day.Day).ToList()
                }).ToList() ?? new List<PickupLocationResponseDto>()
            };
        }

        return new OrderCalculateResponseDto
        {
            ListOrderItens = listItens,
            Seller = sellerDto,
            Total = listItens.Sum(item => item.TotalPrice),
            Fee = 0 
        };
    
    }
    
    public static void ToUpdateEntity(OrderReservationEntity orderEntity, OrderReservationUpdateDto orderReservationUpdateDto,
        ICollection<OrderReservationItemEntity> listOrderItem,  decimal reservationFee)
    {
        orderEntity.UpdateOrderStatus(MapStatus(orderReservationUpdateDto.OrderStatus));
    }
    public static OrderReservationEntity ToCreateEntity(OrderReservationCreateDto orderReservationCreateDto,
        OrderReservationDetailsDto detail,
        PickupLocationEntity pickupLocationEntity,
        decimal reservationFee,
        Guid sellerId)
    {
        try
        {
            if (!String.IsNullOrEmpty(orderReservationCreateDto.FullName) && orderReservationCreateDto.UserId == null)
            {
                return OrderReservationEntity.CreateForGuest(
                    orderReservationCreateDto.FullName,
                    orderReservationCreateDto.Email,
                    orderReservationCreateDto.PhoneNumber,
                    pickupLocationEntity,
                    detail.PickupDate,
                    detail.PickupDeadline,
                    reservationFee,
                    sellerId);
            }
            
            return OrderReservationEntity.CreateForCustomer(
                orderReservationCreateDto!.UserId.Value,
                PickupLocationMapper.ToEntity(detail.PickupLocation),
                detail.PickupDate,
                detail.PickupDeadline,
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