using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Test.Controllers;

[TestClass]
public class RestockingControllerTests
{
    private static RestockingController GetController(Mock<IStockRepository> stockRepository) =>
        new(stockRepository.Object);

    [TestMethod]
    public async Task RestockTomatoes()
    {
        // Arrange
        StockDto tomato = new(StockType.Tomatoes, 1);
        ComparableList<StockDto> stock = [tomato];

        Mock<IStockRepository> stockRepository = new(MockBehavior.Strict);
        stockRepository.Setup(x => x.AddToStock(tomato)).ReturnsAsync(tomato);

        RestockingController controller = GetController(stockRepository);

        // Act
        IActionResult actual = await controller.Restock(stock);

        // Assert
        Assert.IsInstanceOfType<OkResult>(actual);
    }
}
