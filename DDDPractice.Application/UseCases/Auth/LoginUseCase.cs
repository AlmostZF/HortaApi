using DDD_Practice.DDDPractice.Domain.Repositories;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Interface;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Shared;
using Microsoft.AspNetCore.Identity;

namespace DDDPractice.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenService _tokenService;
    
    public LoginUseCase(
        IAuthRepository authRepository,
        ITokenService tokenService)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResponseDto>> ExecuteAsync(LoginDto login)
    {
        try
        {
            var user = await _authRepository.FindByEmailAsync(login.Email);

            if (user == null)
                return Result<AuthResponseDto>.Failure("não autorizado", 404);

            if (!await _authRepository.CheckPasswordAsync(user, login.Password))
                return Result<AuthResponseDto>.Failure("não autorizado", 404);

            user.Roles = await _authRepository.GetRolesAsync(user);

            var token = await _tokenService.CreateTokenAsync(user);

            return Result<AuthResponseDto>.Success(token,200);
        }
        catch (Exception e)
        {
            return Result<AuthResponseDto>.Failure(e.Message,500);
        }

    }
}