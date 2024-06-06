using PizzaPlace.Models.Types;

namespace PizzaPlace.Services;

public class MenuService : IMenuService
{
    public Task<Menu> GetMenu(DateTimeOffset menuDate)
    {
        ComparableList<MenuItem> itemsLunch =
        [
            new("Lunch Pizza", PizzaRecipeType.HorseRadishPizza, 12.5)
        ];
        var menuLunch = new Menu("It's lunch, boy", itemsLunch);

        ComparableList<MenuItem> itemsNotLunch =
        [
            new("Non-Lunch Pizza", PizzaRecipeType.StandardPizza, 18.0)
        ];

        var menuNotLunch = new Menu("It ain't lunch, boy", itemsNotLunch);

        if (menuDate.Hour >= 11 && menuDate.Hour < 14)
        {
            return Task.FromResult(menuLunch);
        }

        return Task.FromResult(menuNotLunch);
    }
}
