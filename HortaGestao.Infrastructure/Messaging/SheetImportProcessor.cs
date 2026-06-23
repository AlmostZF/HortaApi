using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.UseCases.CreateProductWithStockUseCase;
using HortaGestao.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace HortaGestao.Infrastructure.Interfaces;

public class SheetImportProcessor: ISheetImportProcessor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CreateProductBulkWithStocUseCase _createProductBulkWithStock;


    public SheetImportProcessor(CreateProductBulkWithStocUseCase createProductBulkWithStock)
    {
        _createProductBulkWithStock = createProductBulkWithStock;
    }

    public async Task ProcessBatchAsync(ImportMessagingDto importData,
        IHubContext<ImportHub> hubContext)
    {

        try
        {
            var importResult = await _createProductBulkWithStock.ExecuteAsync(importData, hubContext);
            Console.WriteLine(importResult);
            await hubContext.Clients.User(importData.UserId.ToString())
                .SendAsync("ReceiveImportComplete", new { importData = importResult});
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
