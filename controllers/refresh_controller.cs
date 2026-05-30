using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using nomination_api.internal_methods;
using nomination_api.DataBaseContext;  
using nomination_api.models;

[ApiController]
[Route("api/[controller]/[action]")]
public class RefreshController : ControllerBase
{
    private readonly DatabaseContext _context; 
    public RefreshController(DatabaseContext context)
    {
        _context = context; 
    }

    [HttpPost]
    [Authorize]
    public Boolean CheckAuth()
    {
        return true;
    }

    private User? FindUser(string username)
    {
       return _context.Users.FirstOrDefault(u => u.UserName == username);
    }
    private User? FindUserWithGUID(Guid guid)
    {
       return _context.Users.FirstOrDefault(u => u.UserId == guid);
    }
    private bool UserExists(Guid id)
    {
        return _context.Users.Any(e => e.UserId == id);
    }
}