using System.Security.Claims;
using HortaGestao.Application.UseCases.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HortaGestao.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class DasboardController:ControllerBase
{
    private readonly GetDashboardOverviewUseCase _getDashboardUseCase;

    public DasboardController(GetDashboardOverviewUseCase getDashboardUseCase)
    {
        _getDashboardUseCase = getDashboardUseCase;
    }
    
    [Authorize]
    [HttpGet("{month}/{year}/{limit}")]
    public async Task<IActionResult> Get([FromRoute] int month, int year, int limit)
    {
        var stringcurrentUserId = User.FindFirst("sub")?.Value 
                                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(stringcurrentUserId, out Guid currentUserId);
        
        var result = await _getDashboardUseCase.ExecuteAsync(currentUserId, month, year, limit);
        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
}