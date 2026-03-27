using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Services;
using HortaGestao.Application.Shared;

namespace HortaGestao.Application.UseCases.MessagingLog;

public class CreateMessagingLogUseCase
{

    private readonly IMessagingLogService _logService;
    
    public CreateMessagingLogUseCase(IMessagingLogService logService)
    {
        _logService = logService;
    }

    public async Task<Result<MessagingLogDto>> ExecuteAsync(ImportMessagingDto importMessagingDto, string messageError)
    {
        try
        {
            var result = await _logService.SaveLogAsync(importMessagingDto, messageError);
            return Result<MessagingLogDto>.Success(result, 200);
        }
        catch (Exception e)
        {
            return Result<MessagingLogDto>.Failure("erro ao salvar Log");
        }
    }
}