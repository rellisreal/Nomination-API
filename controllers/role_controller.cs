using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using nomination_api.DataBaseContext;
using nomination_api.models;
using nomination_api.internal_methods;

[ApiController]
[Authorize]
[Route("api/[controller]/[action]")]
public class RoleController : ControllerBase
{
    private readonly DatabaseContext _context;
    private EventManager eventManager = new EventManager(); 
    public RoleController(DatabaseContext context)
    {
        _context=context;
    }

    //GET: api/Role
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        return await _context.Roles.ToListAsync();
    }
    //GET: api/Role/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Role>> GetRole(Guid id)
    {
        var role=await _context.Roles.FindAsync(id);
        if(role==null){
            return NotFound();
        }
        return role;
    }
    //POST: api/Role
    [HttpPost]
    public async Task<ActionResult<Role>> PostRole(Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        string? user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid user_guid = new Guid(user_id);
        if (_context.Users.Any(u => u.UserId == user_guid))
        {
            User? user_action = _context.Users.FirstOrDefault(u => u.UserId == user_guid);
            eventManager.AddRoleEvent("Add", role, user_action, _context);
        }

        return CreatedAtAction(nameof(GetRole),new{id=role.RoleId}, role);
    }

    //PUT:api/Role/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Role>> PutRole(Guid id, Role role)
    {
        if(id != role.RoleId)
        {
            return BadRequest();
        }

        _context.Entry(role).State=EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException)
        {
            if(!RoleExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        string? user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid user_guid = new Guid(user_id);
        if (_context.Users.Any(u => u.UserId == user_guid))
        {
            User? user_action = _context.Users.FirstOrDefault(u => u.UserId == user_guid);
            eventManager.AddRoleEvent("Edit", role, user_action, _context);
        }

        return Ok(role);
    }

    //DELETE: api/Todo/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRole(Guid id)
    {
        var role=await _context.Roles.FindAsync(id);
        if(role==null)
        {
            return NotFound();
        }
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();

        string? user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid user_guid = new Guid(user_id);
        if (_context.Users.Any(u => u.UserId == user_guid))
        {
            User? user_action = _context.Users.FirstOrDefault(u => u.UserId == user_guid);
            eventManager.AddRoleEvent("Delete", role, user_action, _context);
        }

        return NoContent();
    }
    private bool RoleExists(Guid id)
    {
        return _context.Roles.Any(e => e.RoleId == id);
    }
}