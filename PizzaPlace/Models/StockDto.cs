using Microsoft.EntityFrameworkCore;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Models
{
    public record StockDto(StockType StockType, int Amount, long Id = 0) : Dto(Id);

    public class StockDtoDBContext : DbContext
    {
        public DbSet<StockDto> Stock { get; set; }
    }
}
