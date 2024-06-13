using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace PizzaPlace.Models
{
    public partial class PizzaContext : DbContext
    {
        public PizzaContext() { }

        public PizzaContext(DbContextOptions<PizzaContext> options) : base(options) { }

        public virtual DbSet<RecipeDto> RecipeDtos { get; set; }
        public virtual DbSet<StockDto> StockDtos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=(localdb)\\MSSQLLocalDB;" +
                    "Initial Catalog=PizzaDatabase;" +
                    "Integrated Security=True;" +
                    "Connect Timeout=30;" +
                    "Encrypt=False;" +
                    "Trust Server Certificate=False;" +
                    "Application Intent=ReadWrite;" +
                    "Multi Subnet Failover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
                        
            modelBuilder.Entity<RecipeDto>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasMany(e => e.StockDto).WithMany();

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.CookingTimeMinutes).IsRequired();
                entity.Property(e => e.RecipeType).IsRequired();
            });

            modelBuilder.Entity<StockDto>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.StockType).IsRequired();
                entity.Property(e => e.Amount).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
