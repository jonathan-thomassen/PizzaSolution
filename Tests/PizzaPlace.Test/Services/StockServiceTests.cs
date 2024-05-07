using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services;

[TestClass]
public class StockServiceTests
{
    private static StockService GetService(Mock<IStockRepository> stockRepository) =>
        new(stockRepository.Object);

    [TestMethod]
    public async Task GetStockOfRecipe()
    {
        // Arrange

        // Act

        // Assert
    }
}
