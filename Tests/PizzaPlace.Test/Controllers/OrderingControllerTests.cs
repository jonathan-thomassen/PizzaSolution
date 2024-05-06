using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Controllers;

[TestClass]
public class OrderingControllerTests
{
    private static OrderingController GetController(Mock<IOrderingService> orderingService) =>
        new(orderingService.Object);

    [TestMethod]
    public async Task PlacePizzaOrder()
    {
        // Arrange
        var order = new PizzaOrder([new PizzaAmount(PizzaRecipeType.EmptyPizza, 1)]);

        var orderingService = new Mock<IOrderingService>(MockBehavior.Strict);
        orderingService.Setup(x => x.HandlePizzaOrder(order))
            .ReturnsAsync([]); // Doesn't matter.

        var controller = GetController(orderingService);

        // Act
        var actual = await controller.PlacePizzaOrder(order);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        orderingService.VerifyAll();
    }
}
