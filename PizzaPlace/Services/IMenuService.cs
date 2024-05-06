namespace PizzaPlace.Services;

public interface IMenuService
{
    Task<Menu> GetMenu(DateTimeOffset menuDate);
}
