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
    
    [AllowAnonymous]
    [HttpGet("{sellerId}/{month}/{year}/{limit}")]
    public async Task<IActionResult> Get([FromRoute] Guid sellerId, int month, int year, int limit)
    {

        var result = await _getDashboardUseCase.ExecuteAsync(sellerId, month, year, limit);
        return result.Value != null
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
}