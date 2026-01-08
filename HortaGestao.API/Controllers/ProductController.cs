using System.Security.Claims;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.UseCases.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HortaGestao.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController: ControllerBase
{
    
    private readonly CreateProductUseCase _createProductUseCase;
    private readonly DeleteProductUseCase _deleteProductUseCase;
    private readonly GetAllProductUseCase _getAllProductUseCase;
    private readonly GetProductUseCase _getProductUseCase;
    private readonly UpdateProductUseCase _updateProductUseCase;
    private readonly FilterProductsUseCase _filterProductsUseCase;
    private readonly UpdateProductStatusUseCase _updateProductStatusUseCase;

    public ProductController(
        CreateProductUseCase createProductUseCase,
        DeleteProductUseCase deleteProductUseCase,
        GetAllProductUseCase getAllProductUseCase,
        GetProductUseCase getProductUseCase,
        UpdateProductUseCase updateProductUseCase,
        UpdateProductStatusUseCase updateProductStatusUseCase,
        FilterProductsUseCase filterProductsUseCase)
    {
        _createProductUseCase = createProductUseCase;
        _deleteProductUseCase = deleteProductUseCase;
        _getAllProductUseCase = getAllProductUseCase;
        _getProductUseCase = getProductUseCase;
        _updateProductUseCase = updateProductUseCase;
        _filterProductsUseCase = filterProductsUseCase;
        _updateProductStatusUseCase = updateProductStatusUseCase;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute]Guid id)
    {
        var result = await _getProductUseCase.ExecuteAsync(id);

        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getAllProductUseCase.ExecuteAsync();

        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Seller")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute]Guid id)
    {
        var result = await _deleteProductUseCase.ExecuteAsync(id);

        return result.Message != null
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    [AllowAnonymous]
    [Authorize(Roles = "Seller")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]ProductCreateDto productCreateDTO)
    {
        var result = await _createProductUseCase.ExecuteAsync(productCreateDTO);

        return result.Value != Guid.Empty
            ? Created($"/api/products/{result.Value}",result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [AllowAnonymous]
    [Authorize(Roles = "Seller")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody]ProductUpdateDto productUpdateDto)
    {
        var userClaims = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if(string.IsNullOrEmpty(userClaims))
            return Unauthorized();
        
        var currentUserId = Guid.Parse(userClaims);
        productUpdateDto.SellerId = currentUserId;
        
        var result = await _updateProductUseCase.ExecuteAsync(productUpdateDto);

        return result.Message != null
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [AllowAnonymous]
    [Authorize(Roles = "Seller")]
    [HttpPut("status")]
    public async Task<IActionResult> Update([FromBody]ProductUpdateStatusDto publiProductUpdateStatusDto)
    {
        var userClaims = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if(string.IsNullOrEmpty(userClaims))
            return Unauthorized();
        
        var currentUserId = Guid.Parse(userClaims);
        publiProductUpdateStatusDto.SellerId = currentUserId;
        
        var result = await _updateProductStatusUseCase.ExecuteAsync(publiProductUpdateStatusDto);

        return result.Message != null
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [AllowAnonymous]
    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery] ProductFilterDto productFilterDto)
    {
        var result = await _filterProductsUseCase.ExecuteAsync(productFilterDto);
        
        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
}