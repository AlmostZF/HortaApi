using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.ValueObjects;

namespace HortaGestao.Application.Mappers;

public static class CustomerMapper
{
    public static CustomerResponseDto ToDto(CustomerEntity userEntity)
    {
        if (userEntity is null)
            throw new ArgumentNullException(
                nameof(userEntity),
                "A entidade Customer não pode ser nula para mapeamento."
            );
        
        return new CustomerResponseDto()
        {
            Id = userEntity.Id,
            Name = userEntity.Name,
            PhoneNumber = userEntity.PhoneNumber,
            SecurityCode = userEntity.GetSecurityCode()
        };

    }

    public static List<CustomerResponseDto> ToDtoList(IEnumerable<CustomerEntity> userEntity)
    {
        return userEntity.Select(ToDto).ToList();
    }

    
    public static void ApplyUpdate(CustomerEntity customerEntity, CustomerUpdateDto customerUpdateDto)
    {
        if (customerUpdateDto == null)
            throw new ArgumentException("Customer DTO must not be null.");
        
        customerEntity.UpdateContact(customerUpdateDto.Name, customerUpdateDto.PhoneNumber);
    }
    
    public static CustomerEntity ToUpdateEntity(CustomerCreateDto customerCreateDto)
    {
        var securityCode = new SecurityCode(GenerateUniqueCodeFromGuid());
        return new CustomerEntity(customerCreateDto.Name, customerCreateDto.PhoneNumber, securityCode);
    }
    
    public static List<CustomerEntity> ToEntitylist(List<CustomerCreateDto> userDto)
    {
        return userDto.Select(ToUpdateEntity).ToList();
    }

    private static string GenerateUniqueCodeFromGuid(int length = 4)
    {
        return Guid.NewGuid().ToString("N").Substring(0, length);
    }
    
}