using DDDPractice.DDDPractice.Domain.Entities;
using DDDPractice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;

namespace DDDPractice.Application.Mappers;

public static class CustomerMapper
{
    public static CustomerResponseDto ToDto(CustomerEntity userEntity)
    {
        if (userEntity == null) return new CustomerResponseDto(null!);
        
        return new CustomerResponseDto(userEntity.SecurityCode)
        {
            Id = userEntity.Id,
            Name = userEntity.Name,
            PhoneNumber = userEntity.PhoneNumber
        };

    }

    public static List<CustomerResponseDto> ToDtoList(IEnumerable<CustomerEntity> userEntity)
    {
        return userEntity.Select(ToDto).ToList();
    }


    public static CustomerEntity ToEntity(CustomerResponseDto customerResponseDto)
    {
        var securityCode = new SecurityCode(GenerateUniqueCodeFromGuid());
        return new CustomerEntity(customerResponseDto.Name, customerResponseDto.PhoneNumber, securityCode);
    }
    
    public static void ToUpdateEntity(CustomerEntity customerEntity, CustomerUpdateDto customerUpdateDto)
    {
        if (customerUpdateDto == null) return;
        
        customerEntity.SetName(customerUpdateDto.Name);
        customerEntity.SetPhoneNumber(customerUpdateDto.PhoneNumber);
    }
    
    public static CustomerEntity ToCreateEntity(CustomerCreateDTO customerCreateDto)
    {
        var securityCode = new SecurityCode(GenerateUniqueCodeFromGuid());
        return new CustomerEntity(customerCreateDto.Name, customerCreateDto.PhoneNumber, securityCode);
    }
    
    public static List<CustomerEntity> ToEntitylist(List<CustomerResponseDto> userDto)
    {
        return userDto.Select(ToEntity).ToList();
    }

    private static string GenerateUniqueCodeFromGuid(int length = 4)
    {
        return Guid.NewGuid().ToString("N").Substring(0, length);
    }
    
}