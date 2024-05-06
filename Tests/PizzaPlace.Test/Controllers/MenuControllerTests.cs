using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Controllers;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Controllers;

[TestClass]
public class MenuControllerTests
{
    private static MenuController GetController(TimeProvider timeProvider, Mock<IMenuService> menuService) =>
        new(timeProvider, menuService.Object);

    [TestMethod]
    public async void GetMenu()
    {
        // Arrange
        var time = new DateTimeOffset(2030, 10, 12, 0, 0, 0, TimeSpan.Zero);
        var timeProvider = new FakeTimeProvider(time);
        var menu = new Menu("Just a test menu", []);

        var menuService = new Mock<IMenuService>(MockBehavior.Strict);
        menuService.Setup(x => x.GetMenu(time))
            .ReturnsAsync(menu);

        var controller = GetController(timeProvider, menuService);

        // Act
        var actual = await controller.GetMenu();

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        menuService.VerifyAll();
        var returnedMenu = (actual as OkObjectResult)?.Value as Menu;
        Assert.AreEqual(menu, returnedMenu);
    }
}
