using System.Security.Claims;
using DDDPractice.DDDPractice.Domain.Enums;
using DDDPractice.DDDPractice.Domain.ValueObjects;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Shared;
using DDDPractice.Application.UseCases.OrderReservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDDPractice.API.Controllers;

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

    public OrderReservationController(
        CreateOrderUseCase createOrderUseCase,
        DeleteOrderUseCase deleteOrderUseCase,
        GetAllOrderUseCase getAllOrderUseCase,
        GetOrderBySecurityCodeUseCase getOrderBySecurityUseCase,
        GetOrderByStatusUseCase getOrderByStatusUseCase,
        GetOrderUseCase getOrderUseCase,
        UpdateOrderUseCase updateOrderUseCase,
        CalculateOrderUseCase calculateOrderUseCase
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
    }
    [Authorize(Roles = "Seller,Customer")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {

            var result = await _getOrderUseCase.ExecuteAsync(id);
            return result.Value != null
                ? Ok(result.Value)
                : StatusCode(result.StatusCode, result.Error);
    }
    //[Authorize(Roles = "Admin")]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {

        var result = await _getAllOrderUseCase.ExecuteAsync();
        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Seller,Customer")]
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus([FromRoute] StatusOrder status)
    {

        var result = await _getOrderByStatusUseCase.ExecuteAsync(status);
        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Seller,Customer")]
    [HttpGet("securitycode/{securityCode}")]
    public async Task<IActionResult> GetBySecutityCode([FromRoute] string securityCode)
    {

        var result = await _getOrderBySecurityUseCase.ExecuteAsync(securityCode);
        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }

    [Authorize(Roles = "Seller,Customer")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _deleteOrderUseCase.ExecuteAsync(id);
        return result.Message != null
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderReservationCreateDTO orderReservationCreateDto)
    {
        // if (User.Identity?.IsAuthenticated == true)
        // {
        //     var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //
        //     if (!string.IsNullOrEmpty(userClaim) && Guid.TryParse(userClaim, out Guid currentUserId))
        //     {
        //         orderReservationCreateDto.UserId = currentUserId;
        //     }
        // }
        var result = await _createOrderUseCase.ExecuteAsync(orderReservationCreateDto);
        
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.StatusCode, result.Error);
    }
    
    [HttpPost("pending")]
    public async Task<IActionResult> Calculate([FromBody]OrderCalculateDTO orderCalculateDto)
    {
        var result = await _calculateOrderUseCase.ExecuteAsync(orderCalculateDto);
        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
    
    [Authorize(Roles = "Seller,Customer")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody]OrderReservationUpdateDTO orderReservationUpdateDto)
    {
        var result = await _updateOrderUseCase.ExecuteAsync(orderReservationUpdateDto);
        return result.Message != null
            ? Ok(result.Message)
            : StatusCode(result.StatusCode, result.Error);
    }
}