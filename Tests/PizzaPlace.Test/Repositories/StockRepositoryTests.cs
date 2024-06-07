using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Test.Repositories;

[TestClass]
public class StockRepositoryTests
{
    private static StockRepository GetStockRepository() => new();

    [TestMethod]
    public async Task AddToStock()
    {
        // Arrange
        int addedAmount = 10;
        Stock stock = new Stock(StockType.TrippleBacon, addedAmount);
        StockRepository repository = GetStockRepository();

        // Act
        Stock actual = await repository.AddToStock(stock);

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
        var stock1 = new Stock(StockType.UnicornDust, addedAmount);
        var stock2 = new Stock(StockType.UnicornDust, secondAddedAmount);
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
        var stock = new Stock(StockType.TrippleBacon, addedAmount);
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
        Stock? actual = await repository.GetStock(stockType);

        // Assert
        Assert.AreEqual(stockType, actual?.StockType);
    }

    [TestMethod]
    public async Task GetStock_WithAddToStock()
    {
        // Arrange
        int addedAmount = 233;
        StockType stockType = StockType.GenericSpices;
        StockRepository repository = GetStockRepository();
        // Ensure the stock type is added.
        await repository.AddToStock(new Stock(stockType, 123));
        Stock? startStock = await repository.GetStock(stockType);
        Stock? expected = startStock != null
            ? startStock with { Amount = startStock.Amount + addedAmount } : null;

        // Act
        await repository.AddToStock(new Stock(stockType, addedAmount));
        Stock? actual = await repository.GetStock(stockType);

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
        // Ensure stock is present.
        await repository.AddToStock(new Stock(stockType, amount + 5));
        var expected = new Stock(stockType, amount);

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
        await repository.AddToStock(new Stock(stockType, 5)); // Ensure stock is present.

        // Act
        var ex = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => repository.TakeStock(stockType, amount));

        // Assert
        Assert.AreEqual("Unable to take zero or negative amount. (Parameter 'amount')", ex.Message);
    }

    [TestMethod]
    public async Task TakeStock_NotEnoughStock()
    {
        // Arrange
        StockType stockType = StockType.FermentedDough;
        StockRepository repository = GetStockRepository();
        await repository.AddToStock(new Stock(stockType, 5)); // Ensure stock is present.
        Stock? startStock = await repository.GetStock(stockType);
        int amount = startStock != null ? startStock.Amount + 1 : 0;

        // Act
        PizzaException ex = await Assert.ThrowsExceptionAsync<PizzaException>(
            () => repository.TakeStock(stockType, amount));

        // Assert
        Assert.AreEqual("Not enough stock to take the given amount.", ex.Message);
    }

    [TestMethod]
    public async Task TakeStock_GetStock()
    {
        // Arrange
        StockType stockType = StockType.FermentedDough;
        int amount = 7;
        StockRepository repository = GetStockRepository();
        // Ensure stock is present.
        await repository.AddToStock(new Stock(stockType, amount + 8));
        Stock? startStock = await repository.GetStock(stockType);
        Stock? expected =
            startStock != null ? startStock with { Amount = startStock.Amount - amount } : null;

        // Act
        await repository.TakeStock(stockType, amount);
        Stock? actual = await repository.GetStock(stockType);

        // Assert
        Assert.AreEqual(expected, actual);
    }
}
