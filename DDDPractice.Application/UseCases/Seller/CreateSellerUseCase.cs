using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Services;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.UseCases.Seller;

public class CreateSellerUseCase
{
    private readonly SellerService _sellerService;

    public CreateSellerUseCase(SellerService sellerService)
    {
        _sellerService = sellerService;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(SellerCreateDTO sellerCreateDto)
    {
        try
        {
           var guid = await _sellerService.AddAsync(sellerCreateDto);
           return Result<Guid>.Success(guid,201);
        }
        catch (Exception e)
        {
            return Result<Guid>.Failure("Erro ao crair vendedor", 500);
        }
    }
}
