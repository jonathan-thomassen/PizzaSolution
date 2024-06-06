namespace PizzaPlace.Models
{
    public record PizzaOrder(ComparableList<PizzaAmount> RequestedOrder);
}
