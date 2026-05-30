using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using nomination_api.internal_methods;
using nomination_api.DataBaseContext;  
using nomination_api.models;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthenticationController : ControllerBase
{
    private readonly DatabaseContext _context; 
    private EventManager eventManager = new EventManager();
    public AuthenticationController(DatabaseContext context)
    {
        _context = context; 
    }

    [HttpPost("{username}/{password}")]
    public async Task<ActionResult<Boolean>> Login(string username, string password)
    {
        var user_account = FindUser(username); 
        if (user_account == null)
        {
            return NotFound();
        }


        var passhash = new PasswordHasher();  
        string hashed_password = passhash.HashPassword(password);
        
        if (hashed_password == user_account.UserPassword) {

            // Create the identity for the user
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user_account.UserId.ToString()) 
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(true);
        }   
        return Unauthorized();


    }
    [HttpPost("{username}/{password}")]
    public async Task<ActionResult<Boolean>> LoginAdmin(string username, string password)
    {
        var user_account = FindUser(username); 
        if (user_account == null || user_account.Role == null)
        {
            return NotFound();
        }


        var passhash = new PasswordHasher();  
        string hashed_password = passhash.HashPassword(password);
        if (hashed_password == user_account.UserPassword && user_account.Role.RoleName.Equals("Admin")){

            // Create the identity for the user
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user_account.UserId.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            eventManager.AdminLoginEvent("Admin Login", user_account, _context);
            
            return Ok(true);
        }   
        return Unauthorized();
    }
    [HttpPost]
    public Boolean Logout()
    {
        var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return true; 
    }

    private User? FindUser(string username)
    {
       return _context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserName == username);
    }    private bool UserExists(Guid id)
    {
        return _context.Users.Any(e => e.UserId == id);
    }
}