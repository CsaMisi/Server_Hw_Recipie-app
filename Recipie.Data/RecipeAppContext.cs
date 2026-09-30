using Microsoft.EntityFrameworkCore;
using Recipie.Model.Entity;

namespace Recipie.Data;

public class RecipeAppContext : DbContext
{
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<DishIngredient> DishIngredients { get; set; }

    public RecipeAppContext(DbContextOptions<RecipeAppContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dish>()
            .HasMany(d => d.DishIngredients)
            .WithOne(di => di.Dish)
            .HasForeignKey(di => di.DishId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Ingredient>()
            .HasMany(i => i.DishIngredients)
            .WithOne(di => di.Ingredient)
            .HasForeignKey(di => di.IngredientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DishIngredient>()
            .HasKey(di => new { di.DishId, di.IngredientId });

        base.OnModelCreating(modelBuilder);
    }
}
