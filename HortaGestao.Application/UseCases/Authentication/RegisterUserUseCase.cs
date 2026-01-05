using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Interfaces.UnitOfWork;
using HortaGestao.Application.Shared;
using HortaGestao.Application.UseCases.Customer;
using HortaGestao.Application.UseCases.Seller;

namespace HortaGestao.Application.UseCases.Authentication;

public class RegisterUserUseCase
{
    private readonly IAuthRepository _authRepository;
    private readonly CreateSellerUseCase _createSellerUseCase;
    private readonly CreateCustomerUseCase _createCustomerUseCase;
    private readonly IUnitOfWork _ouw;

    public RegisterUserUseCase(
        IAuthRepository authRepository,
        CreateSellerUseCase createSellerUseCase,
        CreateCustomerUseCase createCustomerUseCase,
        IUnitOfWork ouw)
    {
        _authRepository = authRepository;
        _createSellerUseCase = createSellerUseCase;
        _createCustomerUseCase = createCustomerUseCase;
        _ouw = ouw;
    }

    public async Task<Result<Guid>> ExecuteAsync(ICreateUserDto dto, UserType type)
    {
        await _ouw.BeginTransactionAsync();
        try
        {
            var domainUser = type switch
            {
                UserType.Customer => await _createCustomerUseCase.ExecuteAsync((CustomerCreateDto)dto),
                UserType.Seller => await _createSellerUseCase.ExecuteAsync((SellerCreateDto)dto),
                _ => Result<Guid>.Failure("Tipo de usuário inválido", 400)
            };

            if (!domainUser.IsSuccess)
            {
                await _ouw.RollbackAsync();
                return domainUser;
            }
            
            var registerDto = new RegisterDto
            {
                Email = dto.Email,
                Password = dto.Password,
                Name = dto.Name
            };
            
            var authUserDto = await _authRepository.CreateAsync(registerDto, domainUser.Value);
            
            if (authUserDto == null)
            {
                await _ouw.RollbackAsync();
                return Result<Guid>.Failure("Erro ao criar usuário de autenticação.", 400);
            }
            
            await _authRepository.AddToRoleAsync(authUserDto.Id.ToString(), type.ToString());

            await _ouw.CommitAsync();
            return Result<Guid>.Success(authUserDto.Id, 201);
        }
        catch (Exception e)
        {   
            await _ouw.RollbackAsync();

            return Result<Guid>.Failure("Erro ao registrar usuário.",500);
        }
    }
}