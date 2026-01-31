using System.Security.Claims;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.UseCases.PickupLocation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HortaGestao.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class PickupLocationController:ControllerBase
{
    
    CreatePickupLocationUseCase _createPickupLocationUseCase;
    UpdatePickupLocationUseCase _updatePickupLocationUseCase;
    DeletePickupLocationUseCase _deletePickupLocationUseCase;
    GetByIdPickupLocationUseCase  _getByIdPickupLocationUseCase;

    public PickupLocationController(CreatePickupLocationUseCase createPickupLocationUseCase,
        UpdatePickupLocationUseCase updatePickupLocationUseCase,
        DeletePickupLocationUseCase deletePickupLocationUseCase,
        GetByIdPickupLocationUseCase getByIdPickupLocationUseCase)
    {
        _createPickupLocationUseCase = createPickupLocationUseCase;
        _updatePickupLocationUseCase = updatePickupLocationUseCase;
        _deletePickupLocationUseCase = deletePickupLocationUseCase;
        _getByIdPickupLocationUseCase = getByIdPickupLocationUseCase;
    }
    
    [Authorize(Policy = "SellerRights")]
    [HttpGet]
    public async Task<IActionResult> GetById()
    {
        var stringcurrentUserId = User.FindFirst("sub")?.Value 
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(stringcurrentUserId, out Guid currentUserId);
        
        var result = await _getByIdPickupLocationUseCase.ExecuteAsync(currentUserId);
        
        return result.IsSuccess
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }

    [Authorize(Policy = "SellerRights")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]PickupLocationCreateDto pickupLocationCreateDto)
    {
        var result = await _createPickupLocationUseCase.ExecuteAsync(pickupLocationCreateDto);

        return result.Value != Guid.Empty
            ? Created($"/api/pickupLocation/{result.Value}",result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Policy = "SellerRights")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody]List<PickupLocationUpdateDto> pickupLocationUpdateDto)
    {
        var stringcurrentUserId = User.FindFirst("sub")?.Value 
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(stringcurrentUserId, out Guid currentUserId);
        var result = await _updatePickupLocationUseCase.ExecuteAsync(pickupLocationUpdateDto, currentUserId);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Policy = "SellerRights")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute]Guid id)
    {
        var result = await _deletePickupLocationUseCase.ExecuteAsync(id);

        return result.IsSuccess
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    

    
    

}