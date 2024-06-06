using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Factories
{
    /// <summary>
    /// Producing one line of pizza. 
    /// Taking 7 minutes to setup - and then 5 minutes less for every subsequent
    /// pizza of the same recipe type to a minimum of 4 minutes.
    /// </summary>
    public class AssemblyLinePizzaOven(TimeProvider timeProvider) : PizzaOven(timeProvider)
    {
        private const int AssemblyLineCapacity = 1;
        public const int SetupTimeMinutes = 7;
        public const int SubsequentPizzaTimeSavingsInMinutes = 5;
        public const int MinimumCookingTimeMinutes = 4;

        private PizzaRecipeType? _previousRecipeType = null;
        private int _timeSavingsMinutes = SubsequentPizzaTimeSavingsInMinutes;

        protected override int Capacity => AssemblyLineCapacity;

        protected override void PlanPizzaMaking(
            IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
        {
            foreach ((PizzaRecipeDto recipe, Guid orderGuid) in recipeOrders)
            {
                _pizzaQueue.Enqueue((MakePizza(recipe), orderGuid));
            }
        }

        private Func<Task<Pizza?>> MakePizza(PizzaRecipeDto recipe) => async () =>
        {
            if (recipe.RecipeType == _previousRecipeType)
            {
                int cookingTimeMinutes =
                    SetupTimeMinutes + recipe.CookingTimeMinutes - _timeSavingsMinutes;
                if (cookingTimeMinutes < MinimumCookingTimeMinutes)
                {
                    cookingTimeMinutes = MinimumCookingTimeMinutes;
                }
                _timeSavingsMinutes += SubsequentPizzaTimeSavingsInMinutes;
                await CookPizza(cookingTimeMinutes);
            }
            else
            {
                await CookPizza(SetupTimeMinutes + recipe.CookingTimeMinutes);
                _timeSavingsMinutes = SubsequentPizzaTimeSavingsInMinutes;
            }

            _previousRecipeType = recipe.RecipeType;

            return GetPizza(recipe.RecipeType);
        };
    }
}
