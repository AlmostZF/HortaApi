using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Interface;

namespace DDDPractice.Application.Interfaces;

public interface ITokenService
{
    Task<AuthResponseDto> CreateTokenAsync(AuthUserDto user);
}