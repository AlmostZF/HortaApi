using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;

namespace HortaGestao.Application.Services;

public interface IMessagingLogService

{
    public Task<MessagingLogDto> SaveLogAsync(ImportMessagingDto importMessagingDto, string messageError);
}