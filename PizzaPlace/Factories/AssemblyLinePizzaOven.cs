using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Factories;

/// <summary>
/// Producing one line of pizza. 
/// Taking 7 minutes to setup - and then 5 minutes less for every subsequent pizza of the same recipe type to a minimum of 4 minutes.
/// </summary>
public class AssemblyLinePizzaOven(TimeProvider timeProvider) : PizzaOven(timeProvider)
{
    private const int AssemblyLineCapacity = 1;
    public const int SetupTimeMinutes = 7;
    public const int SubsequentPizzaTimeSavingsInMinutes = 5;
    public const int MinimumCookingTimeMinutes = 4;

    private PizzaRecipeType? _previousRecipeType = null;
    private bool _ovenReady = false;

    protected override int Capacity => AssemblyLineCapacity;

    protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
    {
        foreach (var (recipe, orderGuid) in recipeOrders)
        {
            _pizzaQueue.Enqueue((MakePizza(recipe), orderGuid));
        }
    }

    private Func<Task<Pizza?>> MakePizza(PizzaRecipeDto recipe) => async () =>
    {
        if (!_ovenReady)
        {
            _previousRecipeType = null;
            await Task.Delay(TimeSpan.FromMinutes(SetupTimeMinutes), timeProvider);
            _ovenReady = true;
        }

        if (recipe.RecipeType == _previousRecipeType)
        {
            var cookingTimeMinutes = recipe.CookingTimeMinutes - SubsequentPizzaTimeSavingsInMinutes;
            if (cookingTimeMinutes < MinimumCookingTimeMinutes)
            {
                cookingTimeMinutes = MinimumCookingTimeMinutes;
            }
            await CookPizza(cookingTimeMinutes);
        } else
        {
            await CookPizza(recipe.CookingTimeMinutes);
        }
        
        _previousRecipeType = recipe.RecipeType;

        return GetPizza(recipe.RecipeType);
    };
}
