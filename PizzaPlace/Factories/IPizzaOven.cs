using PizzaPlace.Models;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Factories;

public interface IPizzaOven
{
    Task<IEnumerable<Pizza>> PreparePizzas(ComparableList<PizzaPrepareOrder> order, ComparableList<StockDto> stock);
}
