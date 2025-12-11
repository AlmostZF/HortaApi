using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Services;
using DDDPractice.Application.Shared;
using DDDPractice.Application.UseCases.Seller;
using Microsoft.AspNetCore.Mvc;

namespace DDDPractice.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SellerController: ControllerBase
{
    private readonly CreateSellerUseCase _createSellerUseCase;
    private readonly GetAllSellerUseCase _getAllSellerUseCase;
    private readonly GetSellerUseCase _getSellerUseCase;
    private readonly DeleteSellerUseCase _deleteSellerUseCase;
    private readonly UpdateSellerUseCase _updateSellerUseCase;
    

    public SellerController(
        CreateSellerUseCase createSellerUseCase,
        GetAllSellerUseCase getAllSellerUseCase,
        DeleteSellerUseCase deleteSellerUseCase,
        UpdateSellerUseCase updateSellerUseCase,
        GetSellerUseCase getSellerUseCase
        )
    {
        _createSellerUseCase = createSellerUseCase;
        _getAllSellerUseCase = getAllSellerUseCase;
        _deleteSellerUseCase = deleteSellerUseCase;
        _updateSellerUseCase = updateSellerUseCase;
        _getSellerUseCase = getSellerUseCase;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> getById([FromRoute] Guid id)
    {
        var result = await _getSellerUseCase.ExecuteAsync(id);

        return result.Value != null
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }
    
    [HttpGet]
    public async Task<IActionResult> getAll()
    {

        var result = await _getAllSellerUseCase.ExecuteAsync();

        return result.Value != null
            ? Ok(result.Value)
            : BadRequest(result.Error);
        
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _deleteSellerUseCase.ExecuteAsync(id);

        return result.Message != null
            ? Ok(result.Message)
            : BadRequest(result.Error);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SellerCreateDTO sellerCreateDto)
    {
        var result = await _createSellerUseCase.ExecuteAsync(sellerCreateDto);

        return result.Value != null
            ? Ok(result.Value)
            : BadRequest(result.Error);  
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] SellerUpdateDTO sellerUpdateDto)
    {
        var result = await _updateSellerUseCase.ExecuteAsync(sellerUpdateDto);

        return result.Message != null
            ? Ok(result.Message)
            : BadRequest(result.Error);
    }
}