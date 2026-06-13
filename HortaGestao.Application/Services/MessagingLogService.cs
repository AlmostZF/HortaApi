using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.Services;

public class MessagingLogService : IMessagingLogService
{
    
    public Task<MessagingLogDto> GetLogAsync(ImportMessagingDto importMessagingDto, string messageError)
    {
        var result = new MessagingLogDto
        {
            CreatedAt = DateTime.Now,
            LineError = importMessagingDto.Line,
            MessageError = messageError,
            ProductsDto = importMessagingDto.Products
        };

        return Task.FromResult(result);
    }
}