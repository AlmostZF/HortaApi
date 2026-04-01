using HortaGestao.Application.DTOs.Response;

public interface ISheetImportProcessor
{
    Task<MessagingLogDto> ProcessMessageAsync(string messageContent);
}