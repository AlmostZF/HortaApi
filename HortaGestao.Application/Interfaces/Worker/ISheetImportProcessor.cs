using HortaGestao.Application.DTOs.Request;
using HortaGestao.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

public interface ISheetImportProcessor
{
    Task ProcessBatchAsync(ImportMessagingDto ImportData, IHubContext<ImportHub> hubContext);
}
