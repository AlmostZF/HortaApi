using DDD_Practice.DDDPractice.Domain.Repositories;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Shared;
using DDDPractice.Application.UseCases.Seller;

namespace DDDPractice.Application.UseCases.Auth;

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

    public async Task<Result<Guid>> ExecuteAsync(ICreateUserDTO dto)
    {
        await _ouw.BeginTransactionAsync();
        try
        {
            var domainUser = dto.UserType switch
            {
                UserType.Customer => await _createCustomerUseCase.ExecuteAsync((CustomerCreateDTO)dto),
                UserType.Seller => await _createSellerUseCase.ExecuteAsync((SellerCreateDTO)dto),
                _ => Result<Guid>.Failure("Tipo de usuário inválido", 400)
            };

            var registerDto = new RegisterDTO
            {
                Email = dto.Email,
                Password = dto.Password,
                Name = dto.Name
            };
            
            var authUserDto = await _authRepository.CreateAsync(registerDto, domainUser.Value);

            if (authUserDto == null)
            {
                await _ouw.RollbackAsync();
                return Result<Guid>.Failure("Erro ao criar usuário de autenticação.");
            }
            
            await _authRepository.AddToRoleAsync(authUserDto.Id.ToString(), dto.UserType.ToString());

            await _ouw.CommitAsync();

            return Result<Guid>.Success(authUserDto.Id, 201);
        }
        catch (Exception e)
        {   
            await _ouw.RollbackAsync();
            return Result<Guid>.Failure("Erro ao registrar usuário.");
        }
    }
}