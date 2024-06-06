using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Time.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PizzaPlace.Controllers;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Controllers
{
    [TestClass]
    public class MenuControllerTests
    {
        private static MenuController GetController(
            TimeProvider timeProvider, Mock<IMenuService> menuService) =>
            new(timeProvider, menuService.Object);

        [TestMethod]
        public async void GetMenu()
        {
            // Arrange
            DateTimeOffset time = new(2030, 10, 12, 0, 0, 0, TimeSpan.Zero);
            FakeTimeProvider timeProvider = new(time);
            Menu menu = new("Just a test menu", []);

            Mock<IMenuService> menuService = new(MockBehavior.Strict);
            menuService.Setup(x => x.GetMenu(time))
                .ReturnsAsync(menu);

            MenuController controller = GetController(timeProvider, menuService);

            // Act
            IActionResult actual = await controller.GetMenu();

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(actual);
            menuService.VerifyAll();
            Menu? returnedMenu = (actual as OkObjectResult)?.Value as Menu;
            Assert.AreEqual(menu, returnedMenu);
        }
    }
}
