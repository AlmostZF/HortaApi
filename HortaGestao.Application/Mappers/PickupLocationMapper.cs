using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Application.Mappers;

public class PickupLocationMapper
{
    public static PickupLocationResponseDto ToDto(PickupLocationEntity pickupLocationEntity)
    {
        return new PickupLocationResponseDto()
        {
            Id =  pickupLocationEntity.Id,
            City = pickupLocationEntity.Address.City,
            Number = pickupLocationEntity.Address.Number,
            Street = pickupLocationEntity.Address.Street,
            ZipCode = pickupLocationEntity.Address.ZipCode,
            State = pickupLocationEntity.Address.State,
            Neighborhood = pickupLocationEntity.Address.Neighborhood,
            PickupDays = pickupLocationEntity.AvailablePickupDays
                .Select(d => d.Day)
                .ToList()
        };

    }

    public static List<PickupLocationResponseDto> ToDtoList(IEnumerable<PickupLocationEntity> pickupLocationEntities)
    {
        return pickupLocationEntities.Select(ToDto).ToList();
    }
    
    public static PickupLocationEntity ToEntity(PickupLocationCreateDto pickupLocationCreateDto)
    {
        var address = new Address(
            pickupLocationCreateDto.Street,
            pickupLocationCreateDto.Number,
            pickupLocationCreateDto.City,
            pickupLocationCreateDto.ZipCode,
            pickupLocationCreateDto.State,
            pickupLocationCreateDto.Neighborhood
        );
        
        var availableDays = pickupLocationCreateDto.PickupDays.Select(day => new PickupDay(day));
        
        return new PickupLocationEntity(address, availableDays);
    }
    
    public static void ToUpdateEntity(PickupLocationEntity pickupLocationEntity, PickupLocationUpdateDto pickupLocationUpdateDto)
    {
        if (!string.IsNullOrWhiteSpace(pickupLocationUpdateDto.Street))
        {
            var address = new Address(
                pickupLocationUpdateDto.Street,
                pickupLocationUpdateDto.Number,
                pickupLocationUpdateDto.City,
                pickupLocationUpdateDto.ZipCode,
                pickupLocationUpdateDto.State,
                pickupLocationUpdateDto.Neighborhood
            );

            pickupLocationEntity.ChangeAddress(address);
        }

        if (pickupLocationUpdateDto.PickupDays != null)
        {
            pickupLocationEntity.SetAvailableDays(
                pickupLocationUpdateDto.PickupDays.Select(d => new PickupDay(d))
            );
        }
    }
    
    public static PickupLocationEntity ToCreateEntity(PickupLocationCreateDto pickupLocationCreateDto )
    {
        var address = new Address(
            pickupLocationCreateDto.Street,
            pickupLocationCreateDto.Number,
            pickupLocationCreateDto.City,
            pickupLocationCreateDto.ZipCode,
            pickupLocationCreateDto.State,
            pickupLocationCreateDto.Neighborhood
        );

        var pickupPoint = new PickupLocationEntity(
            address,
            pickupLocationCreateDto.PickupDays.Select(d => new PickupDay(d))
        );
        
        return pickupPoint;
    }
    
    
    public static List<PickupLocationEntity> ToEntitylist(List<PickupLocationCreateDto> pickupLocationCreateDto)
    {
        return pickupLocationCreateDto.Select(ToEntity).ToList();
    }
    
}