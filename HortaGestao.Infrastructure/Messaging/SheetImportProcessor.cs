using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.UseCases.CreateProductWithStockUseCase;
using HortaGestao.Application.UseCases.MessagingLog;
using HortaGestao.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace HortaGestao.Infrastructure.Interfaces;

public class SheetImportProcessor: ISheetImportProcessor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ErrorQueuePublisher _errorWorker;
    private readonly CreateMessagingLogUseCase _createMessagingLog;
    private readonly CreateProductBulkWithStocUseCase _createProductBulkWithStock;


    public SheetImportProcessor(ErrorQueuePublisher errorWorker, CreateMessagingLogUseCase createMessagingLog,
        CreateProductBulkWithStocUseCase createProductBulkWithStock)
    {
        _errorWorker = errorWorker;
        _createMessagingLog = createMessagingLog;
        _createProductBulkWithStock = createProductBulkWithStock;
    }

    public async Task ProcessBatchAsync(ImportMessagingDto importData,
        IHubContext<Messaging.ImportHub> hubContext)
    {

        try
        {
            await _createProductBulkWithStock.ExecuteAsync(importData.Products, importData.UserId);
            await hubContext.Clients.User(importData.UserId.ToString())
                .SendAsync("ReceiveImportComplete", new { ImportId = importData.ImportId });
        }
        catch (Exception e)
        {
            await hubContext.Clients.User(importData.UserId.ToString())
                .SendAsync("ReceiveImportError", new
                {
                    ImportId = importData.ImportId,
                    ErrorMessage = $"Erro ao salvar o lote no banco: {e.Message}"
                });
        }
    }
    
}
