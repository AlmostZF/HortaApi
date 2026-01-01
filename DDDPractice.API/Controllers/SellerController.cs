using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.UseCases.Seller;
using Microsoft.AspNetCore.Authorization;
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
    
    //[Authorize] 
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> getById([FromRoute] Guid id)
    {
        var result = await _getSellerUseCase.ExecuteAsync(id);

        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    //[Authorize(Roles = "Admin")] 
    [Authorize] 
    [HttpGet]
    public async Task<IActionResult> getAll()
    {

        var result = await _getAllSellerUseCase.ExecuteAsync();

        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
        
    }
    
    [Authorize(Roles = "Seller")]
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _deleteSellerUseCase.ExecuteAsync(id);

        return result.Message != null
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Seller")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SellerCreateDTO sellerCreateDto)
    {
        var result = await _createSellerUseCase.ExecuteAsync(sellerCreateDto);

        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Seller")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] SellerUpdateDTO sellerUpdateDto)
    {
        var result = await _updateSellerUseCase.ExecuteAsync(sellerUpdateDto);

        return result.Message != null
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
}