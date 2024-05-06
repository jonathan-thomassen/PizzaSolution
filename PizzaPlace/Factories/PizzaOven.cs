using PizzaPlace.Extensions;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Factories;

public abstract class PizzaOven(TimeProvider timeProvider) : IPizzaOven
{
    protected abstract int Capacity { get; }
    protected readonly ConcurrentQueue<(Func<Task<Pizza?>>, Guid)> _pizzaQueue = new();
    private readonly Dictionary<Guid, Task<(Pizza?, Guid)>> _pizzasInOven = [];
    private readonly object _ovenLock = new();

    public async Task<IEnumerable<Pizza>> PreparePizzas(ComparableList<PizzaPrepareOrder> order, ComparableList<StockDto> stock)
    {
        if (!stock.HasEnough(order.GetRequiredStock()))
            throw new PizzaException("Not enough ingredients to create all pizzas.");

        List<(PizzaRecipeDto Recipe, Guid OrderGuid)> recipeOrders = order
            .SelectMany(order => Enumerable.Range(0, order.OrderAmount).Select(_ => (order.RecipeDto, Guid.NewGuid())))
            .ToList();

        var readyPizzas = new List<Pizza>();
        var waitingOrders = recipeOrders
            .Select(x => x.OrderGuid)
            .ToHashSet();
        PlanPizzaMaking(recipeOrders);

        while (waitingOrders.Count != 0)
        {
            var (readyPizza, orderGuid) = await GetNextReadyPizza();

            if (readyPizza is null)
            {
                TakePizza(orderGuid);
            }
            else if (waitingOrders.Contains(orderGuid))
            {
                readyPizzas.Add(readyPizza);
                waitingOrders.Remove(orderGuid);
                TakePizza(orderGuid);
            }
        }

        AddPizzaTasks();

        return readyPizzas;
    }

    protected abstract void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders);

    protected Task CookPizza(int cookingTimeMinutes) =>
        Task.Delay(TimeSpan.FromMinutes(cookingTimeMinutes), timeProvider);

    protected Pizza GetPizza(PizzaRecipeType recipeType) => recipeType switch
    {
        PizzaRecipeType.StandardPizza => new StandardPizza(),
        PizzaRecipeType.ExtremelyTastyPizza => new ExtremelyTastyPizza(),
        _ => throw new NotSupportedException($"Currently unable to make pizza for a recipe of type {recipeType}"),
    };

    private void TakePizza(Guid orderGuid)
    {
        lock (_ovenLock)
        {
            _pizzasInOven.Remove(orderGuid);
        }
    }

    private async Task<(Pizza?, Guid)> GetNextReadyPizza()
    {
        AddPizzaTasks();
        var donePizza = await Task.WhenAny(_pizzasInOven.Values);
        return await donePizza;
    }

    private void AddPizzaTasks()
    {
        while (AddPizzaTask())
        {
            // Adding pizza task.
        }
    }

    private bool AddPizzaTask()
    {
        lock (_ovenLock)
        {
            if (_pizzasInOven.Count < Capacity && _pizzaQueue.TryDequeue(out var value))
            {
                var (pizzaTaskCreator, orderGuid) = value;
                _pizzasInOven[orderGuid] = Wrap(pizzaTaskCreator(), orderGuid);

                return true;
            }
            else
            {
                return false;
            }
        }

        static async Task<(Pizza?, Guid)> Wrap(Task<Pizza?> pizzaTask, Guid orderGuid) =>
            (await pizzaTask, orderGuid);
    }
}
