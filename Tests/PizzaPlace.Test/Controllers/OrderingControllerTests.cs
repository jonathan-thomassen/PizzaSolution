using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        PizzaOrder order = new([new(PizzaRecipeType.EmptyPizza, 1)]);

        Mock<IOrderingService> orderingService = new(MockBehavior.Strict);
        orderingService.Setup(x => x.HandlePizzaOrder(order)).ReturnsAsync([]); // Doesn't matter.

        OrderingController controller = GetController(orderingService);

        // Act
        IActionResult actual = await controller.PlacePizzaOrder(order);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        orderingService.VerifyAll();
    }
}
