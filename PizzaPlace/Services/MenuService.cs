namespace PizzaPlace.Services;

public class MenuService : IMenuService
{
    public async Task<Menu> GetMenu(DateTimeOffset menuDate)
    {
        var itemsLunch = new ComparableList<MenuItem>
        {
            new MenuItem("Lunch Pizza", Models.Types.PizzaRecipeType.HorseRadishPizza, 12.5)
        };
        var menuLunch = new Menu("It's lunch, boy", itemsLunch);

        var itemsNotLunch = new ComparableList<MenuItem>
        {
            new MenuItem("Non-Lunch Pizza", Models.Types.PizzaRecipeType.StandardPizza, 18.0)
        };
        var menuNotLunch = new Menu("It ain't lunch, boy", itemsNotLunch);

        if (menuDate.Hour >= 11 && menuDate.Hour < 14)
        {
            return menuLunch;
        }

        return menuNotLunch;
    }
}
 