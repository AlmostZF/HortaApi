using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Shared;
using DDDPractice.Application.UseCases.Stock;
using Microsoft.AspNetCore.Mvc;

namespace DDDPractice.API.Controllers;

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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getAllStockUseCase.ExecuteAsync();

        return result.Value != null
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }
    

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _getProductStockUseCase.ExecuteAsync(id);

        return result.Value != null
            ? Ok(result.Value)
            : BadRequest(result.Error);

    }
    [HttpGet("product/{id:guid}")]
    public async Task<IActionResult> GetByProductId([FromRoute] Guid id)
    {
        var result = await _getStockByProductIdUseCase.ExecuteAsync(id);

        return result.Value != null
            ? Ok(result.Value)
            : BadRequest(result.Error);

    }
    

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StockCreateDTO stockCreateDto)
    {
        var result = await _createStockUseCase.ExecuteAsync(stockCreateDto);

        return result.Message != null
            ? Created()
            : BadRequest(result.Error);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] StockUpdateDTO stockUpdateDto)
    {
        var result = await _updateStockUseCase.ExecuteAsync(stockUpdateDto);
        
        return result.Message != null
            ? Ok(result.Message)
            : BadRequest(result.Error);
    }
}