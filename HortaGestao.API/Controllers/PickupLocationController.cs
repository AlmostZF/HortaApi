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
    
    [Authorize(Roles = "Seller, Admin")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute]Guid id)
    {
        var result = await _getByIdPickupLocationUseCase.ExecuteAsync(id);
        
        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }

    [Authorize(Roles = "Seller, Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]PickupLocationCreateDto pickupLocationCreateDto)
    {
        var result = await _createPickupLocationUseCase.ExecuteAsync(pickupLocationCreateDto);

        return result.Value != Guid.Empty
            ? Created($"/api/pickupLocation/{result.Value}",result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Seller, Admin")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody]PickupLocationUpdateDto pickupLocationUpdateDto)
    {

        var result = await _updatePickupLocationUseCase.ExecuteAsync(pickupLocationUpdateDto);

        return result.Message != null
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Seller, Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute]Guid id)
    {
        var result = await _deletePickupLocationUseCase.ExecuteAsync(id);

        return result.Message != null
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    

    
    

}