using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Request.ProductCreateDTO;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Mappers;
using HortaGestao.Domain.Repositories;
using HortaGestao.Domain.ValueObjects;

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
 
    public async Task<List<StockResponseDto>> GetAllAsync()
    {
        var stockList = await _stockRepository.GetAllAsync();
        return StockMapper.ToDtoList(stockList);
    }

    public async Task<StockResponseDto> GetByIdAsync(Guid stockId)
    {
        var stock = await _stockRepository.GetByIdAsync(stockId);

        if (stock == null)
            throw new Exception("Erro ao buscar stock por Id");

        stock.Total = StockMoney.CalculateTotal(stock.Product.UnitPrice, stock.Quantity).Amount;
        var stockDto = StockMapper.ToDto(stock);

        return stockDto;
    }

    public async Task UpdateQuantityAsync(StockUpdateDto stockUpdateDto)
    {
        var stock = await _stockRepository.GetByIdAsync(stockUpdateDto.Id);
        if (stock == null)
            throw new Exception("Stock not found.");
    
        StockMapper.ToUpdateEntity(stock,stockUpdateDto);
        await _stockRepository.UpdateQuantityAsync(stock);
    }

    public async Task<StockAvailableResponseDto> GetByProductIdAsync(Guid productId)
    {
        var stock = await _stockRepository.GetByProductIdAsync(productId);
        
        if (stock == null)
            throw new Exception("Erro ao buscar stock por Id do produto ");
        
        var stockAvailableDto = StockMapper.ToAvailableDto(stock);

        return stockAvailableDto;
    }

    public async Task AddAsync(StockCreateDto stockCreateDto)
    {
        var stockEntityWithProduct = await _stockRepository.GetByProductIdAsync(stockCreateDto.ProductId);
        if (stockEntityWithProduct != null )
            throw new Exception("Stock already had product.");
    
        var productEntity = await _productRepository.GetByIdAsync(stockCreateDto.ProductId);
        if (productEntity == null)
            throw new Exception("Product not found.");

        var stockEntity = StockMapper.ToCreateEntity(stockCreateDto, productEntity.UnitPrice);
        await _stockRepository.AddAsync(stockEntity);
    }
    
}