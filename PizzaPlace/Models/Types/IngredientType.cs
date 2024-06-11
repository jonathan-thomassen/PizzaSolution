using System.Text.Json.Serialization;

namespace PizzaPlace.Models.Types
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum IngredientType
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
}
