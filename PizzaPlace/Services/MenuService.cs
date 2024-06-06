namespace PizzaPlace.Services;

public class MenuService : IMenuService
{
    public async Task<Menu> GetMenu(DateTimeOffset menuDate)
    {
        ComparableList<MenuItem> itemsLunch =
        [
            new("Lunch Pizza",
                Models.Types.PizzaRecipeType.HorseRadishPizza,
                12.5)
        ];
        var menuLunch = new Menu("It's lunch, boy", itemsLunch);

        ComparableList<MenuItem> itemsNotLunch =
        [
            new("Non-Lunch Pizza",
                Models.Types.PizzaRecipeType.StandardPizza,
                18.0)
        ];

        var menuNotLunch = new Menu("It ain't lunch, boy", itemsNotLunch);

        if (menuDate.Hour >= 11 && menuDate.Hour < 14)
        {
            return menuLunch;
        }

        return menuNotLunch;
    }
}
