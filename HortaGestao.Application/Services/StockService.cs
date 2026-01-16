
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Mappers;
using HortaGestao.Domain.IRepositories;

namespace HortaGestao.Application.Services;

public class StockService : IStockService
{
    private readonly IStockRepository _stockRepository;
    private readonly IProductRepository _productRepository;

    public StockService(IStockRepository stockRepository, IProductRepository productRepository)
    {
        _stockRepository = stockRepository;
        _productRepository = productRepository;
    }
 
    public async Task<List<StockResponseDto>> GetAllAsync(Guid sellerId)
    {
        var stockList = await _stockRepository.GetAllAsync(sellerId);
        return StockMapper.ToDtoList(stockList);
    }

    public async Task<StockResponseDto> GetByIdAsync(Guid stockId, Guid sellerId)
    {
        var stock = await _stockRepository.GetByIdAsync(stockId, sellerId);

        if (stock == null)
            throw new Exception("Erro ao buscar stock por Id");

        stock.CalculateTotal(stock.Product.UnitPrice, stock.Quantity);
        var stockDto = StockMapper.ToDto(stock);

        return stockDto;
    }

    public async Task UpdateQuantityAsync(StockUpdateDto stockUpdateDto, Guid sellerId)
    {
        var stock = await _stockRepository.GetByIdAsync(stockUpdateDto.Id, sellerId);
        if (stock == null)
            throw new Exception("Stock not found.");
    
        StockMapper.ToUpdateEntity(stock,stockUpdateDto);
        await _stockRepository.UpdateQuantityAsync(stock);
    }

    public async Task<StockAvailableResponseDto> GetByProductIdAsync(Guid productId, Guid sellerId)
    {
        var stock = await _stockRepository.GetByProductIdAsync(productId, sellerId);
        
        if (stock == null)
            throw new Exception("Erro ao buscar stock por Id do produto ");
        
        var stockAvailableDto = StockMapper.ToAvailableDto(stock);

        return stockAvailableDto;
    }

    public async Task AddAsync(StockCreateDto stockCreateDto, Guid sellerId)
    {
        var stockEntityWithProduct = await _stockRepository.GetByProductIdAsync(stockCreateDto.ProductId, sellerId);
        if (stockEntityWithProduct != null )
            throw new Exception("Stock already had product.");
    
        var productEntity = await _productRepository.GetByIdAsync(stockCreateDto.ProductId);
        if (productEntity == null)
            throw new Exception("Product not found.");

        var stockEntity = StockMapper.ToCreateEntity(stockCreateDto, productEntity.UnitPrice);
        await _stockRepository.AddAsync(stockEntity);
    }
    
}