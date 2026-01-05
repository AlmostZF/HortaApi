using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Request.ProductCreateDTO;
using HortaGestao.Application.Shared;
using HortaGestao.Application.UseCases.Stock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HortaGestao.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class StockController: ControllerBase
{
    private readonly CreateStockUseCase _createStockUseCase;
    private readonly UpdateQuantityUseCase _updateStockUseCase;
    private readonly GetAllStockUseCase _getAllStockUseCase;
    private readonly GetProductStockUseCase _getProductStockUseCase;
    private readonly GetStockByProductIdUseCase _getStockByProductIdUseCase;

    public StockController(
        CreateStockUseCase createStockUseCase,
        UpdateQuantityUseCase updateStockUseCase,
        GetAllStockUseCase getAllStockUseCase,
        GetProductStockUseCase getProductStockUseCase,
        GetStockByProductIdUseCase getStockByProductIdUseCase)
    {
        _createStockUseCase = createStockUseCase;
        _updateStockUseCase = updateStockUseCase;
        _getAllStockUseCase = getAllStockUseCase;
        _getProductStockUseCase = getProductStockUseCase;
        _getStockByProductIdUseCase = getStockByProductIdUseCase;
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getAllStockUseCase.ExecuteAsync();

        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [AllowAnonymous]
    [Authorize(Roles = "Seller")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _getProductStockUseCase.ExecuteAsync(id);

        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);

    }
    
    [AllowAnonymous]
    [HttpGet("product/{id:guid}")]
    public async Task<IActionResult> GetByProductId([FromRoute] Guid id)
    {
        var result = await _getStockByProductIdUseCase.ExecuteAsync(id);

        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);

    }
    [AllowAnonymous]
    [Authorize(Roles = "Seller")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StockCreateDto stockCreateDto)
    {
        var result = await _createStockUseCase.ExecuteAsync(stockCreateDto);

        return result.Message != null
            ? Created()
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Seller")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] StockUpdateDto stockUpdateDto)
    {
        var result = await _updateStockUseCase.ExecuteAsync(stockUpdateDto);
        
        return result.Message != null
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
}