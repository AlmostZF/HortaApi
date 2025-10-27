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
    
    public async Task<Result> ExecuteAsync(SellerCreateDTO sellerCreateDto)
    {
        try
        {
           await _sellerService.AddAsync(sellerCreateDto);
            return Result.Success("vendedor criado com sucesso",200);
        }
        catch (Exception e)
        {
            return Result.Failure("Erro ao crair vendedor", 500);
        }
    }
}
