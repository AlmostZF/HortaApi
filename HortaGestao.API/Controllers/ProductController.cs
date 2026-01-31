using System.Security.Claims;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.UseCases.Product;
using HortaGestao.Application.UseCases.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HortaGestao.API.Controllers;

[Authorize]
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
    private readonly GetImageUseCase _getImageUseCase;

    public ProductController(
        CreateProductUseCase createProductUseCase,
        DeleteProductUseCase deleteProductUseCase,
        GetAllProductUseCase getAllProductUseCase,
        GetProductUseCase getProductUseCase,
        UpdateProductUseCase updateProductUseCase,
        UpdateProductStatusUseCase updateProductStatusUseCase,
        FilterProductsUseCase filterProductsUseCase,
        GetImageUseCase getImageUseCase)
    {
        _createProductUseCase = createProductUseCase;
        _deleteProductUseCase = deleteProductUseCase;
        _getAllProductUseCase = getAllProductUseCase;
        _getProductUseCase = getProductUseCase;
        _updateProductUseCase = updateProductUseCase;
        _filterProductsUseCase = filterProductsUseCase;
        _updateProductStatusUseCase = updateProductStatusUseCase;
        _getImageUseCase = getImageUseCase;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute]Guid id)
    {
        var result = await _getProductUseCase.ExecuteAsync(id);

        return result.IsSuccess
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [AllowAnonymous]
    [HttpGet("/api/products/{containerName}/{imageString}")]
    public async Task<IActionResult> GetImage([FromRoute]string containerName, string imageString)
    {
        var result = await _getImageUseCase.ExecuteAsync(imageString,containerName);

        return result.IsSuccess
            ? File(result.Value, "image/png")
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getAllProductUseCase.ExecuteAsync();

        return result.IsSuccess
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute]Guid id)
    {
        var result = await _deleteProductUseCase.ExecuteAsync(id);

        return result.IsSuccess
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Policy = "SellerRights")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm]ProductCreateDto productCreateDTO)
    {
        var stringcurrentUserId = User.FindFirst("sub")?.Value 
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(stringcurrentUserId, out Guid currentUserId);
        var result = await _createProductUseCase.ExecuteAsync(productCreateDTO, currentUserId);

        return result.Value != Guid.Empty
            ? Created($"/api/products/{result.Value}",result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Policy = "SellerRights")]
    [HttpPut]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update([FromForm]ProductUpdateDto productUpdateDto)
    {
        var stringcurrentUserId = User.FindFirst("sub")?.Value 
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(stringcurrentUserId, out Guid currentUserId);
        var result = await _updateProductUseCase.ExecuteAsync(productUpdateDto, currentUserId);

        return result.IsSuccess
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Policy = "SellerRights")]
    [HttpPut("status")]
    public async Task<IActionResult> Update([FromBody]ProductUpdateStatusDto publiProductUpdateStatusDto)
    {
        var result = await _updateProductStatusUseCase.ExecuteAsync(publiProductUpdateStatusDto);

        return result.IsSuccess
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [AllowAnonymous]
    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery] ProductFilterDto productFilterDto)
    {
        var result = await _filterProductsUseCase.ExecuteAsync(productFilterDto);
        
        return result.IsSuccess
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
}