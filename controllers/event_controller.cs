using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using nomination_api.DataBaseContext;  
using nomination_api.models;

[ApiController]
[Authorize]
[Route("api/[controller]/[action]")]
public class EventController : ControllerBase
{
    private readonly DatabaseContext _context; 
    public EventController(DatabaseContext context)
    {
        _context=context;
    }

    //GET: api/Event
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        return await _context.Events.ToListAsync();
    }
    //GET: api/Event/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEvent(Guid id)
    {
        var evt=await _context.Events.FindAsync(id);
        if(evt==null){
            return NotFound();
        }
        return evt;
    }
    //POST: api/Event
    [HttpPost]
    public async Task<ActionResult<Event>> PostEvent(Event evt)
    {
        _context.Events.Add(evt);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvent),new{id=evt.EventId}, evt);
    }
}