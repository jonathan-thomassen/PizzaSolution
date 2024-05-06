using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Test.Repositories;

[TestClass]
public class StockRepositoryTests
{
    private static IStockRepository GetStockRepository() => new FakeStockRepository();

    [TestMethod]
    public async Task AddToStock()
    {
        // Arrange
        var addedAmount = 10;
        var stock = new StockDto(StockType.TrippleBacon, addedAmount);
        var repository = GetStockRepository();

        // Act
        var actual = await repository.AddToStock(stock);

        // Assert
        Assert.IsTrue(actual.Amount >= addedAmount);
    }

    [TestMethod]
    public async Task AddToStock_MoreThanOnce()
    {
        // Arrange
        var addedAmount = 10;
        var secondAddedAmount = 13;
        var expectedLeastAmount = addedAmount + secondAddedAmount;
        var stock1 = new StockDto(StockType.UnicornDust, addedAmount);
        var stock2 = new StockDto(StockType.UnicornDust, secondAddedAmount);
        var repository = GetStockRepository();

        // Act
        await repository.AddToStock(stock1);
        var actual = await repository.AddToStock(stock2);

        // Assert
        Assert.IsTrue(actual.Amount >= expectedLeastAmount);
    }

    [TestMethod]
    public async Task AddToStock_NegativeAmount()
    {
        // Arrange
        var addedAmount = -10;
        var stock = new StockDto(StockType.TrippleBacon, addedAmount);
        var repository = GetStockRepository();

        // Act
        var ex = await Assert.ThrowsExceptionAsync<PizzaException>(() => repository.AddToStock(stock));

        // Assert
        Assert.AreEqual("Stock cannot have negative amount.", ex.Message);
    }

    [TestMethod]
    public async Task GetStock()
    {
        // Arrange
        var stockType = StockType.UngenericSpices;
        var repository = GetStockRepository();

        // Act
        var actual = await repository.GetStock(stockType);

        // Assert
        Assert.AreEqual(stockType, actual.StockType);
    }

    [TestMethod]
    public async Task GetStock_WithAddToStock()
    {
        // Arrange
        var addedAmount = 233;
        var stockType = StockType.GenericSpices;
        var repository = GetStockRepository();
        await repository.AddToStock(new StockDto(stockType, 123)); // Ensure the stock type is added.
        var startStock = await repository.GetStock(stockType);
        var expected = startStock with { Amount = startStock.Amount + addedAmount };

        // Act
        await repository.AddToStock(new StockDto(stockType, addedAmount));
        var actual = await repository.GetStock(stockType);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task TakeStock()
    {
        // Arrange
        var stockType = StockType.FermentedDough;
        var amount = 7;
        var repository = GetStockRepository();
        await repository.AddToStock(new StockDto(stockType, amount + 5)); // Ensure stock is present.
        var expected = new StockDto(stockType, amount);

        // Act
        var actual = await repository.TakeStock(stockType, amount);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [DataRow(0)]
    [DataRow(-3)]
    [DataTestMethod]
    public async Task TakeStock_NegativeAmount(int amount)
    {
        // Arrange
        var stockType = StockType.FermentedDough;
        var repository = GetStockRepository();
        await repository.AddToStock(new StockDto(stockType, 5)); // Ensure stock is present.

        // Act
        var ex = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => repository.TakeStock(stockType, amount));

        // Assert
        Assert.AreEqual("Unable to take zero or negative amount. (Parameter 'amount')", ex.Message);
    }

    [TestMethod]
    public async Task TakeStock_NotEnoughStock()
    {
        // Arrange
        var stockType = StockType.FermentedDough;
        var repository = GetStockRepository();
        await repository.AddToStock(new StockDto(stockType, 5)); // Ensure stock is present.
        var startStock = await repository.GetStock(stockType);
        var amount = startStock.Amount + 1;

        // Act
        var ex = await Assert.ThrowsExceptionAsync<PizzaException>(() => repository.TakeStock(stockType, amount));

        // Assert
        Assert.AreEqual("Not enough stock to take the given amount.", ex.Message);
    }

    [TestMethod]
    public async Task TakeStock_GetStock()
    {
        // Arrange
        var stockType = StockType.FermentedDough;
        var amount = 7;
        var repository = GetStockRepository();
        await repository.AddToStock(new StockDto(stockType, amount + 8)); // Ensure stock is present.
        var startStock = await repository.GetStock(stockType);
        var expected = startStock with { Amount = startStock.Amount - amount };

        // Act
        await repository.TakeStock(stockType, amount);
        var actual = await repository.GetStock(stockType);

        // Assert
        Assert.AreEqual(expected, actual);
    }
}
