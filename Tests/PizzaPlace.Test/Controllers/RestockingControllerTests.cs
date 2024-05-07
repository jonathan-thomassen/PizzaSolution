using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

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
        var tomato = new StockDto(StockType.Tomatoes, 1);
        var stock = new ComparableList<StockDto>() { tomato };

        var stockRepository = new Mock<IStockRepository>(MockBehavior.Strict);
        stockRepository.Setup(x => x.AddToStock(tomato)).ReturnsAsync(tomato);

        var controller = GetController(stockRepository);

        // Act
        var actual = await controller.Restock(stock);

        // Assert
        Assert.IsInstanceOfType<OkResult>(actual);
    }
}
