// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PizzaPlace.Models
{
    public class Stock
    {
        public long IngredientId { get; set; }
        public int Amount { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}
