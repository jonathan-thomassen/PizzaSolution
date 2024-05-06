namespace PizzaPlace.Models.Types;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StockType
{
    Dough,
    FermentedDough,
    Tomatoes,
    RottenTomatoes,
    UnicornDust,
    Anchovies,
    BellPeppers,
    GratedCheese,
    UngratedCheese,
    GenericSpices,
    UngenericSpices,
    Sulphur,
    Bacon,
    DoubleBacon,
    TrippleBacon,
    RayOfSunshine,
    Chocolate
}
