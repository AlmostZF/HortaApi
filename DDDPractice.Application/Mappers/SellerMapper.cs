using DDD_Practice.DDDPractice.Domain.Entities;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.Application.Mappers;

public class SellerMapper
{
    public static SellerResponseDto ToDto(SellerEntity sellerEntity)
    {
        return new SellerResponseDto()
        {
            Id = sellerEntity.Id,
            Name = sellerEntity.Name ,
            PhoneNumber = sellerEntity.PhoneNumber,
            PickupLocation = sellerEntity.PickupLocation,
        };

    }

    public static List<SellerResponseDto> ToDtoList(IEnumerable<SellerEntity> sellerEntity)
    {
        return sellerEntity.Select(ToDto).ToList();
    }


    public static SellerEntity ToEntity(SellerResponseDto sellerResponseDto)
    {

        return new SellerEntity(sellerResponseDto.Name, sellerResponseDto.PhoneNumber,
            sellerResponseDto.PickupLocation);
    }
    public static void ToUpdateEntity(SellerEntity sellerEntity, SellerUpdateDTO sellerUpdateDTO)
    {
        sellerEntity.SetName(sellerUpdateDTO.Name);
        sellerEntity.SetPhoneNumber(sellerUpdateDTO.PhoneNumber);
    }
    
    public static SellerEntity ToCreateEntity(SellerCreateDTO sellerCreateDTO)
    {

        return new SellerEntity(sellerCreateDTO.Name, sellerCreateDTO.PhoneNumber, sellerCreateDTO.PickupLocation);
    }
    

    public static List<SellerEntity> ToEntitylist(List<SellerResponseDto> sellerDto)
    {
        return sellerDto.Select(ToEntity).ToList();
    }

}