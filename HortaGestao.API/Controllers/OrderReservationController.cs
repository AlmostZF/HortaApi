using System.Security.Claims;
using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.UseCases.OrderReservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HortaGestao.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class OrderReservationController: ControllerBase
{

    private readonly CreateOrderUseCase _createOrderUseCase;
    private readonly DeleteOrderUseCase _deleteOrderUseCase;
    private readonly GetAllOrderUseCase _getAllOrderUseCase;
    private readonly CalculateOrderUseCase _calculateOrderUseCase;
    private readonly GetOrderBySecurityCodeUseCase _getOrderBySecurityUseCase;
    private readonly GetOrderByStatusUseCase _getOrderByStatusUseCase;
    private readonly GetOrderUseCase _getOrderUseCase;
    private readonly UpdateOrderUseCase _updateOrderUseCase;
    private readonly FinishOrderUserCase _finishOrderUserCase;

    public OrderReservationController(
        CreateOrderUseCase createOrderUseCase,
        DeleteOrderUseCase deleteOrderUseCase,
        GetAllOrderUseCase getAllOrderUseCase,
        GetOrderBySecurityCodeUseCase getOrderBySecurityUseCase,
        GetOrderByStatusUseCase getOrderByStatusUseCase,
        GetOrderUseCase getOrderUseCase,
        UpdateOrderUseCase updateOrderUseCase,
        CalculateOrderUseCase calculateOrderUseCase,
        FinishOrderUserCase finishOrderUserCase
        )
    {
        _createOrderUseCase = createOrderUseCase;
        _deleteOrderUseCase = deleteOrderUseCase;
        _getAllOrderUseCase = getAllOrderUseCase;
        _getOrderBySecurityUseCase = getOrderBySecurityUseCase;
        _getOrderByStatusUseCase = getOrderByStatusUseCase;
        _getOrderUseCase = getOrderUseCase;
        _updateOrderUseCase = updateOrderUseCase;
        _calculateOrderUseCase = calculateOrderUseCase;
        _finishOrderUserCase = finishOrderUserCase;
    }
    
    [Authorize(Policy = "ActiveUser")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var stringcurrentUserId = User.FindFirst("sub")?.Value 
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(stringcurrentUserId, out Guid currentUserId);
    
        var result = await _getOrderUseCase.ExecuteAsync(currentUserId);
        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Policy = "SellerRights")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {

        var result = await _getAllOrderUseCase.ExecuteAsync();
        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Policy = "SellerRights")]
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus([FromRoute] StatusOrder status)
    {
        var result = await _getOrderByStatusUseCase.ExecuteAsync(status);
        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Policy = "ActiveUser")]
    [HttpGet("securitycode/{securityCode}")]
    public async Task<IActionResult> GetBySecutityCode([FromRoute] string securityCode)
    {
        var stringcurrentUserId = User.FindFirst("sub")?.Value 
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(stringcurrentUserId, out Guid currentUserId);
        
        var result = await _getOrderBySecurityUseCase.ExecuteAsync(securityCode, currentUserId);
        return result.IsSuccess
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _deleteOrderUseCase.ExecuteAsync(id);
        return result.IsSuccess
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [AllowAnonymous]
    [Authorize(Policy = "ActiveUser")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderReservationCreateDto orderReservationCreateDto)
    {

        var result = await _createOrderUseCase.ExecuteAsync(orderReservationCreateDto);
        
        return result.IsSuccess ? Ok(result.Value) : StatusCode(result.StatusCode, result.Error);
    }
    
    [AllowAnonymous]
    [HttpPost("pending")]
    public async Task<IActionResult> Calculate([FromBody]OrderCalculateDto orderCalculateDto)
    {
        var result = await _calculateOrderUseCase.ExecuteAsync(orderCalculateDto);
        return result.IsSuccess
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody]OrderReservationUpdateDto orderReservationUpdateDto)
    {
        var result = await _updateOrderUseCase.ExecuteAsync(orderReservationUpdateDto);
        return result.IsSuccess
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    
        
    [Authorize(Policy = "ActiveUser")]
    [HttpPut("finish")]
    public async Task<IActionResult> Finish([FromBody]UpdateStatusOrderDto updateStatusOrderDto)
    {
        var stringcurrentUserId = User.FindFirst("sub")?.Value 
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(stringcurrentUserId, out Guid currentUserId);
        var result = await _finishOrderUserCase.ExecuteAsync(updateStatusOrderDto.Id, currentUserId);
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result.Error);
    }
}