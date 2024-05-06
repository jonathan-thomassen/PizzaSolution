using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Services;

namespace PizzaPlace.Controllers;

[Route("api/menu")]
public class MenuController(TimeProvider timeProvider, IMenuService menuService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMenu()
    {
        return Ok(await menuService.GetMenu(timeProvider.GetUtcNow()));
    }
}
