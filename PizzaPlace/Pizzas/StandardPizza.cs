using PizzaPlace.Models.Types;

namespace PizzaPlace.Pizzas;

public record StandardPizza : Pizza
{
    public StandardPizza() : base(PizzaRecipeType.StandardPizza)
    {
    }
}
