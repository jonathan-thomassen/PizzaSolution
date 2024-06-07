using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace PizzaPlace.Models
{
    public partial class PizzaContext : DbContext
    {
        public PizzaContext() { }

        public PizzaContext(DbContextOptions<PizzaContext> options) : base(options) { }

        public virtual DbSet<PizzaRecipe> Recipes { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<RecipeStock> RecipeStock { get; set; }

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
                        
            modelBuilder.Entity<PizzaRecipe>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasMany(e => e.Stock).WithMany().UsingEntity<RecipeStock>();

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.CookingTimeMinutes).IsRequired();
                entity.Property(e => e.RecipeType).IsRequired();
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.StockType).IsRequired();
                entity.Property(e => e.Amount).IsRequired();
            });

            modelBuilder.Entity<RecipeStock>(entity =>
            {
                entity.HasKey(e => new { e.RecipeId, e.StockId });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
