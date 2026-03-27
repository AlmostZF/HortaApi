using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.Services;

public class MessagingLogService : IMessagingLogService
{
    
    public Task<MessagingLogDto> SaveLogAsync(ImportMessagingDto importMessagingDto, string messageError)
    {
        var result = new MessagingLogDto
        {
            CreatedAt = DateTime.Now,
            LineError = importMessagingDto.Line,
            MessageError = messageError,
            ProductDto = importMessagingDto.Product
        };

        return Task.FromResult(result);
    }
}