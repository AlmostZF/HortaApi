using DDD_Practice.DDDPractice.Domain.Entities;
using DDD_Practice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.Application.Mappers;

public static class UserMapper
{
    public static UserResponseDto ToDto(UserEntity userEntity)
    {
        if (userEntity == null) return new UserResponseDto(null!);
        
        return new UserResponseDto(userEntity.SecurityCode)
        {
            Id = userEntity.Id,
            Name = userEntity.Name,
            PhoneNumber = userEntity.PhoneNumber
        };

    }



    public static List<UserResponseDto> ToDtoList(IEnumerable<UserEntity> userEntity)
    {
        return userEntity.Select(ToDto).ToList();
    }


    public static UserEntity ToEntity(UserResponseDto userResponseDto)
    {
        var securityCode = new SecurityCode(GenerateUniqueCodeFromGuid());
        if (userResponseDto == null) return new UserEntity(null);
        
        return new UserEntity(securityCode)
        {
            Id = userResponseDto.Id ?? Guid.NewGuid(),
            Name = userResponseDto.Name,
            PhoneNumber = userResponseDto.PhoneNumber
        }; 
    }
    public static void ToUpdateEntity(UserEntity userEntity, UserUpdateDto userUpdateDto)
    {
        if (userUpdateDto == null) return;

        {
            userEntity.Id = userUpdateDto.Id;
            userEntity.Name = userUpdateDto.Name;
            userEntity.PhoneNumber = userUpdateDto.PhoneNumber;
        };
    }
    
    public static UserEntity ToCreateEntity(UserCreateDTO userCreateDto)
    {
        var securityCode = new SecurityCode(GenerateUniqueCodeFromGuid());
        if (userCreateDto == null) return new UserEntity(null);
        
        return new UserEntity(securityCode)
        {
            Id = Guid.NewGuid(),
            Name = userCreateDto.Name,
            PhoneNumber = userCreateDto.PhoneNumber
        }; 
    }
    
    public static List<UserEntity> ToEntitylist(List<UserResponseDto> userDto)
    {
        return userDto.Select(ToEntity).ToList();
    }

    private static string GenerateUniqueCodeFromGuid(int length = 4)
    {
        return Guid.NewGuid().ToString("N").Substring(0, length);
    }
    
}