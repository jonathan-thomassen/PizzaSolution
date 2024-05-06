using PizzaPlace.Models.Types;

namespace PizzaPlace.Models;

public record PizzaOrder(ComparableList<PizzaAmount> RequestedOrder);
