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
}
