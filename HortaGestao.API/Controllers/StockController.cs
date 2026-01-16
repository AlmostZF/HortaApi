using System.Security.Claims;
using HortaGestao.Application.DTOs.Request;
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
    
    [Authorize(Policy = "SellerRights")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var stringcurrentUserId = User.FindFirst("sub")?.Value 
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(stringcurrentUserId, out Guid currentUserId);
        var result = await _getAllStockUseCase.ExecuteAsync(currentUserId);

        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Policy = "SellerRights")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var stringcurrentUserId = User.FindFirst("sub")?.Value 
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(stringcurrentUserId, out Guid currentUserId);
        
        var result = await _getProductStockUseCase.ExecuteAsync(id, currentUserId);

        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);

    }
    
    [Authorize(Policy = "SellerRights")] 
    [HttpGet("product/{id:guid}")]
    public async Task<IActionResult> GetByProductId([FromRoute] Guid id)
    {
        var stringcurrentUserId = User.FindFirst("sub")?.Value 
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(stringcurrentUserId, out Guid currentUserId);
        var result = await _getStockByProductIdUseCase.ExecuteAsync(id,currentUserId);

        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);

    }
    
    [Authorize(Policy = "SellerRights")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StockCreateDto stockCreateDto)
    {
        
        var stringcurrentUserId = User.FindFirst("sub")?.Value 
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(stringcurrentUserId, out Guid currentUserId);
        
        var result = await _createStockUseCase.ExecuteAsync(stockCreateDto, currentUserId);

        return result.Message != null
            ? Created()
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Policy = "SellerRights")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] StockUpdateDto stockUpdateDto)
    {
        var stringcurrentUserId = User.FindFirst("sub")?.Value 
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(stringcurrentUserId, out Guid currentUserId);
        
        var result = await _updateStockUseCase.ExecuteAsync(stockUpdateDto, currentUserId);
        
        return result.Message != null
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
}