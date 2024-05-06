using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Controllers;

[Route("api/restocking")]
public class RestockingController(IStockRepository stockRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Restock([FromBody] ComparableList<StockDto> stock)
    {
        throw new NotImplementedException("Restocking has not been implemented.");
    }
}
