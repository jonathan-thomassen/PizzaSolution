using PizzaPlace.Models;

namespace PizzaPlace.Extensions
{
    public static class PizzaHelperExtensions
    {
        public static IEnumerable<Stock> GetRequiredStock(
            this IEnumerable<PizzaPrepareOrder> prepareOrders) => prepareOrders
            .SelectMany(order => Enumerable.Range(0, order.OrderAmount)
            .Select(_ => order.RecipeDto))
            .SelectMany(recipe => recipe.Stock)
            .GatherSameTypeOfStock();

        public static IEnumerable<Stock> GatherSameTypeOfStock(
            this IEnumerable<Stock> stock) => stock
            .GroupBy(stock => stock.StockType)
            .Select(stockGroup => new Stock(stockGroup.Key, stockGroup.Sum(x => x.Amount)));

        public static bool HasEnough(
            this IEnumerable<Stock> availableStock, IEnumerable<Stock> requiredStock) =>
            requiredStock.GatherSameTypeOfStock()
            .All(required => availableStock.Any(
                x => x.StockType == required.StockType && x.Amount >= required.Amount));
    }
}
