using Microsoft.AspNetCore.Mvc;
using Recipie.Model.Dto.Ingredient;
using Recipie.BizLogic;

namespace Recipie.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientController : ControllerBase
{
    private readonly IngredientLogic _logic;

    public IngredientController(IngredientLogic logic)
    {
        _logic = logic;
    }

    [HttpGet]
    public async Task<ActionResult<List<IngredientReadDto>>> GetAll()
    {
        return Ok(await _logic.ReadAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IngredientReadDto>> GetById(string id)
    {
        var ingredient = await _logic.ReadByIdAsync(id);
        if (ingredient is null)
        {
            return NotFound();
        }

        return Ok(ingredient);
    }

    [HttpPost]
    public async Task<ActionResult<IngredientReadDto>> Create([FromBody] IngredientCreateDto dto)
    {
        var created = await _logic.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<IngredientReadDto>> Update(string id, [FromBody] IngredientUpdateDto dto)
    {
        var updated = await _logic.UpdateAsync(id, dto);
        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var deleted = await _logic.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
