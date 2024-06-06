using System.Text.Json.Serialization;

namespace PizzaPlace.Models.Types
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PizzaRecipeType
    {
        StandardPizza,
        ExtremelyTastyPizza,
        OddPizza,
        RarePizza,
        HorseRadishPizza,
        EmptyPizza,
    }
}
