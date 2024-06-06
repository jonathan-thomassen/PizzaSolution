using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Services;

namespace PizzaPlace.Controllers
{
    [Route("api/order")]
    public class OrderingController(IOrderingService orderingService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PlacePizzaOrder([FromBody] PizzaOrder pizzaOrder)
        {
            return Ok(new
            {
                pizzas = await orderingService.HandlePizzaOrder(pizzaOrder),
            });
        }
    }
}
