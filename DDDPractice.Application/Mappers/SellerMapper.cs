using DDD_Practice.DDDPractice.Domain.Entities;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.Application.Mappers;

public class SellerMapper
{
    public static SellerResponseDto ToDto(SellerEntity sellerEntity)
    {
        if (sellerEntity == null) return new SellerResponseDto();
        
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
        if (sellerResponseDto == null) return new SellerEntity(null);
        
        return new SellerEntity(sellerResponseDto.PickupLocation)
        {
            Id = Guid.NewGuid(),
            Name = sellerResponseDto.Name ,
            PhoneNumber = sellerResponseDto.PhoneNumber,
        };
    }
    public static void ToUpdateEntity(SellerEntity sellerEntity, SellerUpdateDTO sellerUpdateDTO)
    {

        sellerEntity.Id = sellerUpdateDTO.Id;
        sellerEntity.Name = sellerUpdateDTO.Name;
        sellerEntity.PhoneNumber = sellerUpdateDTO.PhoneNumber;
    }
    public static SellerEntity ToCreateEntity(SellerCreateDTO sellerCreateDTO)
    {
        if (sellerCreateDTO == null) return new SellerEntity(null);
        
        return new SellerEntity(sellerCreateDTO.PickupLocation)
        {
            Id = Guid.NewGuid(),
            Name = sellerCreateDTO.Name ,
            PhoneNumber = sellerCreateDTO.PhoneNumber,
        };
    }
    

    public static List<SellerEntity> ToEntitylist(List<SellerResponseDto> sellerDto)
    {
        return sellerDto.Select(ToEntity).ToList();
    }

}