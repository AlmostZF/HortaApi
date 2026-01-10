using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.Storage;

public class GetImageUseCase
{
    private readonly IStorageService _storageService;

    public GetImageUseCase(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<Result<byte[]>> ExecuteAsync(string imageString,string containerName)
    {

            if(string.IsNullOrEmpty(imageString))
                return Result<byte[]>.Failure("Nome da imagem inválido", 400);
            
            var imagem = await _storageService.GetFileAsync(imageString, containerName);

            if (imagem == null)
                return Result<byte[]>.Failure("Imagem não encontrada", 404);
            
            return Result<byte[]>.Success(imagem,200);

    }
}