﻿using PizzaPlace.Models.Types;

namespace PizzaPlace.Models
{
    public class PizzaRecipe
    {
        public PizzaRecipeType RecipeType { get; set; }
        public ComparableList<Stock> Stock { get; set; } = [];
        public int CookingTimeMinutes { get; set; }
        public long Id { get; set; }
    }
}