using PizzaPlace.Models.Types;

namespace PizzaPlace.Models
{
    public record Stock(
        StockType StockType,
        int Amount,
        long Id = 0);
}
