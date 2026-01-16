using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Seller;

public class GetSellerUseCase
{
    private readonly ISellerService _sellerService;
    private readonly IAuthRepository _authRepository;

    public GetSellerUseCase(ISellerService sellerService,  IAuthRepository authRepository)
    {
        _sellerService = sellerService;
        _authRepository = authRepository;
    }
    
    public async Task<Result<SellerResponseDto>> ExecuteAsync(Guid id)
    {
        try
        {
            var sellerId = await _authRepository.GetBusinessIdByIdentityIdAsync(id);
            if (sellerId == null)
                return Result<SellerResponseDto>.Failure("Identificação do usuário inválida.", 401);
            
            var sellerDto = await _sellerService.GetByIdAsync(sellerId.Value);
            return Result<SellerResponseDto>.Success(sellerDto,200);
        }
        catch (Exception e)
        {
            return Result<SellerResponseDto>.Failure("Erro ao buscar vendedor", 500);
        }
    }
}