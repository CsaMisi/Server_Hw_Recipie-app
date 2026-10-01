using Microsoft.AspNetCore.Mvc;
using Recipie.Model.Dto.Dish;
using Recipie.BizLogic;

namespace Recipie.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DishController : ControllerBase
{
    private readonly DishLogic _logic;

    public DishController(DishLogic logic)
    {
        _logic = logic;
    }

    [HttpGet]
    public async Task<ActionResult<List<DishReadDto>>> GetAll()
    {
        return Ok(await _logic.ReadAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DishReadDto>> GetById(string id)
    {
        var dish = await _logic.ReadByIdAsync(id);
        if (dish is null)
        {
            return NotFound();
        }

        return Ok(dish);
    }

    [HttpPost]
    public async Task<ActionResult<DishReadDto>> Create([FromBody] DishCreateDto dto)
    {
        var created = await _logic.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DishReadDto>> Update(string id, [FromBody] DishUpdateDto dto)
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
