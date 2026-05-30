using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using nomination_api.DataBaseContext;  
using nomination_api.models;
using nomination_api.internal_methods;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly DatabaseContext _context; 
    public Event eventaction; 
    private EventManager eventManager = new EventManager();
    public UserController(DatabaseContext context)
    {
        _context=context;
    }

    //GET: api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }
    //GET: api/User/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user=await _context.Users.FindAsync(id);
        if(user==null){
            return NotFound();
        }
        return user;
    }
    //POST: api/User
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        var passhash = new PasswordHasher();  
        string hashedpass = passhash.HashPassword(user.UserPassword);
        user.UserPassword = hashedpass;
        user.UserName = user.UserName.ToLower(); 
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        User? newuser = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == user.UserName.ToLower());

        string? adding_user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid user_guid = new Guid(adding_user_id);
        if (UserExists(user_guid))
        {
                User? adding_user =  _context.Users.FirstOrDefault(u => u.UserId == user_guid);
                eventManager.AddUserEvent("Add",newuser,adding_user, _context);
                
        } 

        

        return CreatedAtAction(nameof(GetUser),new{id=user.UserId}, user);
    }


    private bool UserExists(Guid id)
    {
        return _context.Users.Any(e => e.UserId == id);
    }
}