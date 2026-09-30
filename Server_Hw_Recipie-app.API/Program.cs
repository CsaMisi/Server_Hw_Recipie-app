using Microsoft.EntityFrameworkCore;
using Recipie.BizLogic;
using Recipie.Data;

namespace Server_Hw_Recipie_app.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<RecipeAppContext>(options =>
                options.UseSqlite("Data Source=recipeapp.db"));

            builder.Services.AddScoped(typeof(Repository<>));
            builder.Services.AddScoped<DishLogic>();
            builder.Services.AddScoped<IngredientLogic>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RecipeAppContext>();
                context.Database.EnsureCreated();
                SeedData.Initialize(context);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }

    public static class SeedData
    {
        public static void Initialize(RecipeAppContext context)
        {
            if (context.Ingredients.Any())
            {
                return;
            }

            var ingredients = new[]
            {
                new Recipie.Model.Entity.Ingredient { Name = "Tomato", CaloriesPer100g = 18 },
                new Recipie.Model.Entity.Ingredient { Name = "Beef", CaloriesPer100g = 250 },
                new Recipie.Model.Entity.Ingredient { Name = "Pasta", CaloriesPer100g = 350 },
                new Recipie.Model.Entity.Ingredient { Name = "Salt", CaloriesPer100g = 0 },
                new Recipie.Model.Entity.Ingredient { Name = "Sugar", CaloriesPer100g = 400 },
                new Recipie.Model.Entity.Ingredient { Name = "Chicken Breast", CaloriesPer100g = 165 },
                new Recipie.Model.Entity.Ingredient { Name = "Peas", CaloriesPer100g = 81 },
                new Recipie.Model.Entity.Ingredient { Name = "Onion", CaloriesPer100g = 40 }
            };

            context.Ingredients.AddRange(ingredients);
            context.SaveChanges();

            var pasta = context.Ingredients.Single(i => i.Name == "Pasta");
            var tomato = context.Ingredients.Single(i => i.Name == "Tomato");
            var beef = context.Ingredients.Single(i => i.Name == "Beef");
            var onion = context.Ingredients.Single(i => i.Name == "Onion");
            var peas = context.Ingredients.Single(i => i.Name == "Peas");

            var dishes = new[]
            {
                new Recipie.Model.Entity.Dish
                {
                    Name = "Bolognese",
                    DishIngredients = new List<Recipie.Model.Entity.DishIngredient>
                    {
                        new() { Ingredient = beef, QuantityGrams = 200 },
                        new() { Ingredient = tomato, QuantityGrams = 150 },
                        new() { Ingredient = onion, QuantityGrams = 50 },
                        new() { Ingredient = pasta, QuantityGrams = 120 }
                    }
                },
                new Recipie.Model.Entity.Dish
                {
                    Name = "Pea Soup",
                    DishIngredients = new List<Recipie.Model.Entity.DishIngredient>
                    {
                        new() { Ingredient = peas, QuantityGrams = 150 },
                        new() { Ingredient = onion, QuantityGrams = 80 },
                        new() { Ingredient = tomato, QuantityGrams = 100 }
                    }
                }
            };

            context.Dishes.AddRange(dishes);
            context.SaveChanges();
        }
    }
}