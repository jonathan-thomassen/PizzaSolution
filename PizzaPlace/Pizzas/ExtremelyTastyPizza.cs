using PizzaPlace.Models.Types;

namespace PizzaPlace.Pizzas;

public record ExtremelyTastyPizza : Pizza
{
    public ExtremelyTastyPizza() : base(PizzaRecipeType.ExtremelyTastyPizza)
    {
    }
}
