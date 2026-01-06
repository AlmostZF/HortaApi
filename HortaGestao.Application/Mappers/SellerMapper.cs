using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Domain.Entities;

namespace HortaGestao.Application.Mappers;

public class SellerMapper
{
    public static SellerResponseDto ToDto(SellerEntity sellerEntity)
    {
        return new SellerResponseDto()
        {
            Id = sellerEntity.Id,
            Name = sellerEntity.Name ,
            PhoneNumber = sellerEntity.PhoneNumber,
            PickupLocation = PickupLocationMapper.ToDtoList(sellerEntity.PickupLocations)
        };

    }

    public static List<SellerResponseDto> ToDtoList(IEnumerable<SellerEntity> sellerEntity)
    {
        return sellerEntity.Select(ToDto).ToList();
    }
    

    public static SellerEntity ToEntity(SellerResponseDto sellerResponseDto)
    {

        return new SellerEntity(sellerResponseDto.Name, sellerResponseDto.PhoneNumber);
    }
    public static void ToUpdateEntity(SellerEntity sellerEntity, SellerUpdateDto sellerUpdateDTO)
    {
        sellerEntity.SetName(sellerUpdateDTO.Name);
        sellerEntity.SetPhoneNumber(sellerUpdateDTO.PhoneNumber);
    }
    
    public static SellerEntity ToCreateEntity(SellerCreateDto sellerCreateDTO)
    {
        return new SellerEntity(sellerCreateDTO.Name, sellerCreateDTO.PhoneNumber);
    }
    

    public static List<SellerEntity> ToEntitylist(List<SellerResponseDto> sellerDto)
    {
        return sellerDto.Select(ToEntity).ToList();
    }

}