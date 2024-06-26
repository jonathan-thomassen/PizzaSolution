﻿using PizzaPlace.Models.Types;

namespace PizzaPlace.Models
{
    public record StockDto : IngredientBase
    {
        public long Id { get; set; }
        public StockDto(StockType StockType, int Amount, long id = 0) : base(StockType, Amount)
        {
            Id = id;
        }
    }
}
