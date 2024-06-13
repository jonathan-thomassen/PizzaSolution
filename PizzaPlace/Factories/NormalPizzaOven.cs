using PizzaPlace.Models;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Factories
{
    /// <summary>
    /// A normal oven producing each pizza one at a time. Each pizza taking the normal cooking time.
    /// </summary>
    public class NormalPizzaOven(TimeProvider timeProvider) : PizzaOven(timeProvider)
    {
        private const int NormalPizzaOvenCapacity = 4;

        protected override int Capacity => NormalPizzaOvenCapacity;

        protected override void PlanPizzaMaking(
            IEnumerable<(RecipeDto Recipe, Guid Guid)> recipeOrders)
        {
            foreach ((RecipeDto recipe, Guid orderGuid) in recipeOrders)
            {
                _pizzaQueue.Enqueue((MakePizza(recipe), orderGuid));
            }
        }

        private Func<Task<Pizza?>> MakePizza(RecipeDto recipe) => async () =>
        {
            await CookPizza(recipe.CookingTimeMinutes);

            return GetPizza(recipe.RecipeType);
        };
    }
}
