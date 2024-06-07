using PizzaPlace.Models;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Factories
{
    /// <summary>
    /// Produces pizza on one giant revolving surface.Downside is, that all
    /// pizzas must have the same cooking time, when prepared at the same time.
    /// </summary>
    public class GiantRevolvingPizzaOven(TimeProvider timeProvider) : PizzaOven(timeProvider)
    {
        private const int GiantRevolvingPizzaOvenCapacity = 120;

        private int? _previousCookingTime = null;

        protected override int Capacity => GiantRevolvingPizzaOvenCapacity;

        protected override void PlanPizzaMaking(
            IEnumerable<(PizzaRecipe Recipe, Guid Guid)> recipeOrders)
        {
            foreach ((PizzaRecipe recipe, Guid orderGuid) in recipeOrders)
            {
                _pizzaQueue.Enqueue((MakePizza(recipe), orderGuid));
            }
        }

        private Func<Task<Pizza?>> MakePizza(PizzaRecipe recipe) => async () =>
        {
            if (_previousCookingTime == null || recipe.CookingTimeMinutes == _previousCookingTime)
            {
                _previousCookingTime = recipe.CookingTimeMinutes;
                await CookPizza(recipe.CookingTimeMinutes);
            }
            else
            {
                await CookPizza((int)_previousCookingTime);
                await CookPizza(recipe.CookingTimeMinutes);
            }

            return GetPizza(recipe.RecipeType);
        };
    }
}
