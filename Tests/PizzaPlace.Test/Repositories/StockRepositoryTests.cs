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
        StockDto stock = new(StockType.TrippleBacon, addedAmount);
        StockRepository repository = GetStockRepository();

        // Act
        StockDto? actual = await repository.AddToStock(stock);

        // Assert
        Assert.IsTrue(actual?.Amount >= addedAmount);
    }

    [TestMethod]
    public async Task AddToStock_MoreThanOnce()
    {
        // Arrange
        int addedAmount = 10;
        int secondAddedAmount = 13;
        int expectedLeastAmount = addedAmount + secondAddedAmount;
        StockDto stock1 = new(StockType.UnicornDust, addedAmount);
        StockDto stock2 = new(StockType.UnicornDust, secondAddedAmount);
        StockRepository repository = GetStockRepository();

        // Act
        await repository.AddToStock(stock1);
        StockDto? actual = await repository.AddToStock(stock2);

        // Assert
        Assert.IsTrue(actual?.Amount >= expectedLeastAmount);
    }

    [TestMethod]
    public async Task AddToStock_NegativeAmount()
    {
        // Arrange
        int addedAmount = -10;
        StockDto stock = new(StockType.TrippleBacon, addedAmount);
        StockRepository repository = GetStockRepository();

        // Act
        PizzaException ex =
            await Assert.ThrowsExceptionAsync<PizzaException>(() => repository.AddToStock(stock));

        // Assert
        Assert.AreEqual("Stock cannot have negative amount.", ex.Message);
    }

    [TestMethod]
    public async Task GetStock()
    {
        // Arrange
        StockType stockType = StockType.UngenericSpices;
        StockRepository repository = GetStockRepository();

        // Act
        StockDto? actual = await repository.GetStock(stockType);

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
        await repository.AddToStock(new StockDto(stockType, 123));
        StockDto? startStock = await repository.GetStock(stockType);
        StockDto? expected = startStock != null
            ? startStock with { Amount = startStock.Amount + addedAmount } : null;

        // Act
        await repository.AddToStock(new StockDto(stockType, addedAmount));
        StockDto? actual = await repository.GetStock(stockType);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task TakeStock()
    {
        // Arrange
        StockType stockType = StockType.FermentedDough;
        int amount = 7;
        StockRepository repository = GetStockRepository();
        // Ensure stock is present.
        await repository.AddToStock(new(stockType, amount + 5));
        StockDto expected = new(stockType, amount);

        // Act
        StockDto actual = await repository.TakeStock(stockType, amount);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [DataRow(0)]
    [DataRow(-3)]
    [DataTestMethod]
    public async Task TakeStock_NegativeAmount(int amount)
    {
        // Arrange
        StockType stockType = StockType.FermentedDough;
        StockRepository repository = GetStockRepository();
        await repository.AddToStock(new StockDto(stockType, 5)); // Ensure stock is present.

        // Act
        ArgumentOutOfRangeException ex =
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(
                () => repository.TakeStock(stockType, amount));

        // Assert
        Assert.AreEqual("Unable to take zero or negative amount. (Parameter 'amount')", ex.Message);
    }

    [TestMethod]
    public async Task TakeStock_NotEnoughStock()
    {
        // Arrange
        StockType stockType = StockType.FermentedDough;
        StockRepository repository = GetStockRepository();
        await repository.AddToStock(new StockDto(stockType, 5)); // Ensure stock is present.
        StockDto? startStock = await repository.GetStock(stockType);
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
        await repository.AddToStock(new StockDto(stockType, amount + 8));
        StockDto? startStock = await repository.GetStock(stockType);
        StockDto? expected =
            startStock != null ? startStock with { Amount = startStock.Amount - amount } : null;

        // Act
        await repository.TakeStock(stockType, amount);
        StockDto? actual = await repository.GetStock(stockType);

        // Assert
        Assert.AreEqual(expected, actual);
    }
}
