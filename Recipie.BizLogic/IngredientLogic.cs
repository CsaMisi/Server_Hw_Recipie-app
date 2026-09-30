using Microsoft.EntityFrameworkCore;
using Recipie.Data;
using Recipie.Model.Dto.Ingredient;
using Recipie.Model.Entity;

namespace Recipie.BizLogic;

public class IngredientLogic
{
    private readonly Repository<Ingredient> _repository;

    public IngredientLogic(Repository<Ingredient> repository)
    {
        _repository = repository;
    }

    public async Task<List<IngredientReadDto>> ReadAsync()
    {
        return await _repository.GetAll()
            .Select(i => new IngredientReadDto
            {
                Id = i.Id,
                Name = i.Name,
                CaloriesPer100g = i.CaloriesPer100g
            })
            .ToListAsync();
    }

    public async Task<IngredientReadDto?> ReadByIdAsync(string id)
    {
        var ingredient = await _repository.GetByIdAsync(id);
        if (ingredient is null)
        {
            return null;
        }

        return new IngredientReadDto
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            CaloriesPer100g = ingredient.CaloriesPer100g
        };
    }

    public async Task<IngredientReadDto> CreateAsync(IngredientCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("Ingredient name is required.");
        }

        var entity = new Ingredient
        {
            Name = dto.Name.Trim(),
            CaloriesPer100g = dto.CaloriesPer100g
        };

        await _repository.CreateAsync(entity);

        return new IngredientReadDto
        {
            Id = entity.Id,
            Name = entity.Name,
            CaloriesPer100g = entity.CaloriesPer100g
        };
    }

    public async Task<IngredientReadDto?> UpdateAsync(string id, IngredientUpdateDto dto)
    {
        var ingredient = await _repository.GetByIdAsync(id);
        if (ingredient is null)
        {
            return null;
        }

        ingredient.Name = dto.Name.Trim();
        ingredient.CaloriesPer100g = dto.CaloriesPer100g;
        await _repository.UpdateAsync(ingredient);

        return new IngredientReadDto
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            CaloriesPer100g = ingredient.CaloriesPer100g
        };
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var ingredient = await _repository.GetByIdAsync(id);
        if (ingredient is null)
        {
            return false;
        }

        await _repository.DeleteAsync(ingredient);
        return true;
    }
}
