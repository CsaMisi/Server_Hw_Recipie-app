using Microsoft.EntityFrameworkCore;
using Recipie.Data;
using Recipie.Model.Dto.Dish;
using Recipie.Model.Entity;

namespace Recipie.BizLogic;

public class DishLogic
{
    private readonly Repository<Dish> _dishRepository;
    private readonly Repository<Ingredient> _ingredientRepository;

    public DishLogic(Repository<Dish> dishRepository, Repository<Ingredient> ingredientRepository)
    {
        _dishRepository = dishRepository;
        _ingredientRepository = ingredientRepository;
    }

    public async Task<List<DishReadDto>> ReadAsync()
    {
        var dishes = await _dishRepository.GetAll()
            .Include(d => d.DishIngredients)
            .ThenInclude(di => di.Ingredient)
            .ToListAsync();

        return dishes.Select(MapToReadDto).ToList();
    }

    public async Task<DishReadDto?> ReadByIdAsync(string id)
    {
        var dish = await _dishRepository.GetAll()
            .Include(d => d.DishIngredients)
            .ThenInclude(di => di.Ingredient)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dish is null)
        {
            return null;
        }

        return MapToReadDto(dish);
    }

    public async Task<DishReadDto> CreateAsync(DishCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("Dish name is required.");
        }

        var dish = new Dish
        {
            Name = dto.Name.Trim(),
            DishIngredients = new List<DishIngredient>()
        };

        foreach (var item in dto.Ingredients)
        {
            if (string.IsNullOrWhiteSpace(item.IngredientId) || item.QuantityGrams <= 0)
            {
                throw new ArgumentException("Each ingredient needs a valid id and quantity.");
            }

            var ingredient = await _ingredientRepository.GetByIdAsync(item.IngredientId);
            if (ingredient is null)
            {
                throw new KeyNotFoundException($"Ingredient not found: {item.IngredientId}");
            }

            dish.DishIngredients.Add(new DishIngredient
            {
                Dish = dish,
                Ingredient = ingredient,
                DishId = dish.Id,
                IngredientId = ingredient.Id,
                QuantityGrams = item.QuantityGrams
            });
        }

        await _dishRepository.CreateAsync(dish);
        return MapToReadDto(dish);
    }

    public async Task<DishReadDto?> UpdateAsync(string id, DishUpdateDto dto)
    {
        var dish = await _dishRepository.GetAll()
            .Include(d => d.DishIngredients)
            .ThenInclude(di => di.Ingredient)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dish is null)
        {
            return null;
        }

        dish.Name = dto.Name.Trim();
        dish.DishIngredients.Clear();

        foreach (var item in dto.Ingredients)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(item.IngredientId);
            if (ingredient is null)
            {
                throw new KeyNotFoundException($"Ingredient not found: {item.IngredientId}");
            }

            dish.DishIngredients.Add(new DishIngredient
            {
                Dish = dish,
                Ingredient = ingredient,
                DishId = dish.Id,
                IngredientId = ingredient.Id,
                QuantityGrams = item.QuantityGrams
            });
        }

        await _dishRepository.UpdateAsync(dish);
        return MapToReadDto(dish);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var dish = await _dishRepository.GetByIdAsync(id);
        if (dish is null)
        {
            return false;
        }

        await _dishRepository.DeleteAsync(dish);
        return true;
    }

    private static DishReadDto MapToReadDto(Dish dish)
    {
        var items = dish.DishIngredients
            .Select(di =>
            {
                var ingredient = di.Ingredient;
                var caloriesContribution = ingredient.CaloriesPer100g * di.QuantityGrams / 100.0;

                return new DishIngredientReadDto
                {
                    IngredientId = ingredient.Id,
                    IngredientName = ingredient.Name,
                    QuantityGrams = di.QuantityGrams,
                    CaloriesPer100g = ingredient.CaloriesPer100g,
                    CaloriesContribution = caloriesContribution
                };
            })
            .ToList();

        return new DishReadDto
        {
            Id = dish.Id,
            Name = dish.Name,
            Ingredients = items,
            TotalCalories = items.Sum(i => i.CaloriesContribution)
        };
    }
}
