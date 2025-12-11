using DDD_Practice.DDDPractice.Domain.Repositories;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.Auth;

public class RegisterUserUseCase
{
    private readonly IAuthRepository _authRepository;

    public RegisterUserUseCase(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }
    public async Task<Result<Guid>> ExecuteAsync(RegisterDTO registerDto, Guid domainUserId, string role)
    {
        try
        {
            var created = await _authRepository.CreateAsync(registerDto, domainUserId);

            if (created == null)
                return Result<Guid>.Failure("Erro ao criar usuário");
            
            await _authRepository.AddToRoleAsync(created.Id.ToString(), role);

            return Result<Guid>.Success(created.Id, 201);
        }
        catch (Exception e)
        {
            throw new Exception("Erro ao criar usuário");
        }
    }
}