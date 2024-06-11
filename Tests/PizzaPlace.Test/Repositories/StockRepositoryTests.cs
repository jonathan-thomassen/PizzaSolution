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
        Ingredient stock = new(IngredientType.TrippleBacon, addedAmount);
        StockRepository repository = GetStockRepository();

        // Act
        Ingredient actual = await repository.AddToStock(stock);

        // Assert
        Assert.IsTrue(actual.Amount >= addedAmount);
    }

    [TestMethod]
    public async Task AddToStock_MoreThanOnce()
    {
        // Arrange
        int addedAmount = 10;
        int secondAddedAmount = 13;
        int expectedLeastAmount = addedAmount + secondAddedAmount;
        Ingredient stock1 = new(IngredientType.UnicornDust, addedAmount);
        Ingredient stock2 = new(IngredientType.UnicornDust, secondAddedAmount);
        StockRepository repository = GetStockRepository();

        // Act
        await repository.AddToStock(stock1);
        Ingredient actual = await repository.AddToStock(stock2);

        // Assert
        Assert.IsTrue(actual.Amount >= expectedLeastAmount);
    }

    [TestMethod]
    public async Task AddToStock_NegativeAmount()
    {
        // Arrange
        int addedAmount = -10;
        Ingredient stock = new(IngredientType.TrippleBacon, addedAmount);
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
        IngredientType stockType = IngredientType.UngenericSpices;
        StockRepository repository = GetStockRepository();

        // Act
        Ingredient? actual = await repository.GetStock(stockType);

        // Assert
        Assert.AreEqual(stockType, actual?.StockType);
    }

    [TestMethod]
    public async Task GetStock_WithAddToStock()
    {
        // Arrange
        int addedAmount = 233;
        IngredientType stockType = IngredientType.GenericSpices;
        StockRepository repository = GetStockRepository();
        // Ensure the stock type is added.
        await repository.AddToStock(new Ingredient(stockType, 123));
        Ingredient? startStock = await repository.GetStock(stockType);
        Ingredient? expected = startStock != null
            ? new Ingredient(stockType, startStock.Amount + addedAmount) : null;

        // Act
        await repository.AddToStock(new Ingredient(stockType, addedAmount));
        Ingredient? actual = await repository.GetStock(stockType);

        // Assert
        Assert.AreEqual(expected?.StockType, actual?.StockType);
        Assert.AreEqual(expected?.Amount, actual?.Amount);
    }

    [TestMethod]
    public async Task TakeStock()
    {
        // Arrange
        IngredientType stockType = IngredientType.FermentedDough;
        int amount = 7;
        StockRepository repository = GetStockRepository();
        // Ensure stock is present.
        await repository.AddToStock(new(stockType, amount + 5));
        Ingredient expected = new(stockType, amount);

        // Act
        Ingredient actual = await repository.TakeStock(stockType, amount);

        // Assert
        Assert.AreEqual(expected.Amount, actual.Amount);
    }

    [DataRow(0)]
    [DataRow(-3)]
    [DataTestMethod]
    public async Task TakeStock_NegativeAmount(int amount)
    {
        // Arrange
        var stockType = IngredientType.FermentedDough;
        var repository = GetStockRepository();
        await repository.AddToStock(new Ingredient(stockType, 5)); // Ensure stock is present.

        // Act
        var ex = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => repository.TakeStock(stockType, amount));

        // Assert
        Assert.AreEqual("Unable to take zero or negative amount. (Parameter 'amount')", ex.Message);
    }

    [TestMethod]
    public async Task TakeStock_NotEnoughStock()
    {
        // Arrange
        IngredientType stockType = IngredientType.FermentedDough;
        StockRepository repository = GetStockRepository();
        await repository.AddToStock(new Ingredient(stockType, 5)); // Ensure stock is present.
        Ingredient? startStock = await repository.GetStock(stockType);
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
        IngredientType stockType = IngredientType.FermentedDough;
        int amount = 7;
        StockRepository repository = GetStockRepository();
        // Ensure stock is present.
        await repository.AddToStock(new Ingredient(stockType, amount + 8));
        Ingredient? startStock = await repository.GetStock(stockType);
        Ingredient? expected =
            startStock != null ? new Ingredient(stockType, startStock.Amount - amount) : null;

        // Act
        await repository.TakeStock(stockType, amount);
        Ingredient? actual = await repository.GetStock(stockType);

        // Assert
        Assert.AreEqual(expected?.Amount, actual?.Amount);
    }
}
