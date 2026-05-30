using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nomination_api.DataBaseContext;  
using nomination_api.models;
using nomination_api.internal_methods;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


[ApiController]
[Authorize]
[Route("api/[controller]/[action]")]
public class NominationController : ControllerBase
{
    private readonly DatabaseContext _context; 
    private EventManager eventManager = new EventManager();
    public NominationController(DatabaseContext context)
    {
        _context=context;
    }

    //GET: api/Nomination
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Nomination>>> GetNominations()
    {
        return await _context.Nominations.ToListAsync();
    }
    //GET: api/Nomination/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Nomination>> GetNomination(Guid id)
    {
        var nominationItem=await _context.Nominations.FindAsync(id);
        if(nominationItem==null){
            return NotFound();
        }
        return nominationItem;
    }
    //POST: api/Nomination
    [HttpPost]
    public async Task<ActionResult<Nomination>> PostNomination(Nomination nomination)
    {
        var mail = new MailManager();

        string? adding_user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(adding_user_id) || !Guid.TryParse(adding_user_id, out Guid user_guid))
        {
            return Unauthorized();
        }
        if ( _context.Users.Any(e => e.UserId == user_guid))
        {
                User? nominator =  _context.Users.FirstOrDefault(u => u.UserId == user_guid);
                User? nominated =  _context.Users.FirstOrDefault(u => u.UserId == nomination.NominatedId);
                if (nominator != null && nominated != null) {mail.SendNominationEmail(nominator, nominated, nomination);}
                nomination.NominatorId = user_guid;
                _context.Nominations.Add(nomination);
                await _context.SaveChangesAsync();
                eventManager.AddNominationEvent("Add", nomination, _context);

                return CreatedAtAction(nameof(GetNomination),new{id=nomination.NominationId}, nomination);
        } 
        return BadRequest("Adding user was not found.");
    }

    // //PUT:api/Nomination/5
    // [HttpPut("{id}")]
    // public async Task<ActionResult<Nomination>> PutNomination(Guid id, Nomination nomination)
    // {
    //     if(id != nomination.NominationId)
    //     {
    //         return BadRequest();
    //     }

    //     _context.Entry(nomination).State=EntityState.Modified;
    //     try
    //     {
    //         await _context.SaveChangesAsync();
    //     }
    //     catch(DbUpdateConcurrencyException)
    //     {
    //         if(!NominationExists(id))
    //         {
    //             return NotFound();
    //         }
    //         else
    //         {
    //             throw;
    //         }
    //     }
    //     return NoContent();
    // }

    //DELETE: api/Todo/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteNomination(Guid id)
    {
        var nominationItem=await _context.Nominations.FindAsync(id);
        if(nominationItem==null)
        {
            return NotFound();
        }
        _context.Nominations.Remove(nominationItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    private bool NominationExists(Guid id)
    {
        return _context.Nominations.Any(e => e.NominationId == id);
    }
}
