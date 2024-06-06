using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaPlace.Models.Types;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services;

[TestClass]
public class MenuServiceTests
{
    private static MenuService GetService() =>
        new MenuService();

    [TestMethod]
    public async Task GetLunchMenu()
    {
        // Arrange
        var itemsLunch = new ComparableList<MenuItem>
        {
            new MenuItem("Lunch Pizza", PizzaRecipeType.HorseRadishPizza, 12.5)
        };
        Menu expected = new Menu("It's lunch, boy", itemsLunch);
        var time = DateTimeOffset.Parse("12:00");

        var service = GetService();

        // Act
        var actual = await service.GetMenu(time);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task GetNonLunchMenu()
    {
        // Arrange
        var itemsLunch = new ComparableList<MenuItem>
        {
            new MenuItem("Non-Lunch Pizza", PizzaRecipeType.StandardPizza, 18.0)
        };
        Menu expected = new Menu("It ain't lunch, boy", itemsLunch);
        var time = DateTimeOffset.Parse("17:00");

        var service = GetService();

        // Act
        var actual = await service.GetMenu(time);

        // Assert
        Assert.AreEqual(expected, actual);
    }
}
