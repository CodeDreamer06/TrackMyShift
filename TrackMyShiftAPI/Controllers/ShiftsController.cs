using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TrackMyShiftAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{

    protected readonly ShiftDbContext _context;

    public ShiftsController(ShiftDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Shift>>> GetAll() => Ok(await _context.Shifts.ToListAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Shift>> Get(int id)
    {
       var shift = await _context.Shifts.FindAsync(id);
       return shift is null ? NotFound() : Ok(shift);
    }

    [HttpPost]
    public async Task<ActionResult<Shift>> Add(Shift shift)
    {
        _context.Shifts.Add(shift);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = shift.Id }, shift);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Shift shift)
    {
        shift.Id = id;
        _context.Entry(shift).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }

        catch (DbUpdateConcurrencyException)
        {
            if (!ShiftExists(id)) return NotFound();
            else throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);
        if (shift is null) return NotFound();

        _context.Shifts.Remove(shift);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ShiftExists(int id) => _context.Shifts.Any(shift => shift.Id == id);
}
