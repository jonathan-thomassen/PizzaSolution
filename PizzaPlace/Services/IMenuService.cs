namespace PizzaPlace.Services;

public interface IMenuService
{
    Menu GetMenu(DateTimeOffset menuDate);
}
