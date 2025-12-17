using DDD_Practice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.Interfaces;

public interface ICreateUserDTO
{
    string Email { get; }
    string Password { get; }
    string Name { get; }
    UserType UserType { get; set; }

}