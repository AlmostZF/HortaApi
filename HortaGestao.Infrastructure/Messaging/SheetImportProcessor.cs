using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.UseCases.CreateProductWithStockUseCase;
using HortaGestao.Application.UseCases.MessagingLog;
using HortaGestao.Infrastructure.Messaging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

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
        IHubContext<ImportHub> hubContext)
    {
        var createProductWithStock = _serviceProvider.GetRequiredService<CreateProductBulkWithStocUseCase>();
        int total = importData.TotalMessages;
        int current = 0;

        foreach (var product in importData.Products)
        {
            current++;

            double percentage = total > 0 ? (double)current / total * 100 : 0;

            await hubContext.Clients.User(importData.UserId.ToString())
                .SendAsync("ReceiveProgress", new
                {
                    ImportId = importData.ImportId,
                    Current = current,
                    Total = total,
                    Percentage = Math.Round(percentage, 2)
                });
        }

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
    
    
    public class ImportHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"ConnectionId: {Context.ConnectionId}");
            Console.WriteLine($"UserIdentifier: {Context.UserIdentifier}");
        

            foreach (var claim in Context.User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            await base.OnConnectedAsync();
        }
    }
    

    public Task ProcessBatchAsync(ImportMessagingDto ImportData, IHubContext<Messaging.ImportHub> hubContext)
    {
        throw new NotImplementedException();
    }
}
