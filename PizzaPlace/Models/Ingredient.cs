using PizzaPlace.Models.Types;

namespace PizzaPlace.Models
{
    public class Ingredient
    {
        public IngredientType IngredientType { get; set; }
        public int Amount { get; set; }
        public long Id { get; set; } = 0;

        public Ingredient() { }

        public Ingredient(IngredientType ingredientType, int amount, long id = 0)
        {
            IngredientType = ingredientType;
            Amount = amount;
            Id = id;
        }
    }

}
