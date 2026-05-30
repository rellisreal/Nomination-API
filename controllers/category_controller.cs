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
public class CategoryController : ControllerBase
{
    private readonly DatabaseContext _context;
    private EventManager eventManager = new EventManager(); 
    public CategoryController(DatabaseContext context)
    {
        _context=context;
    }

    //GET: api/Category
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }
    //GET: api/Category/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(long id)
    {
        var category=await _context.Categories.FindAsync(id);
        if(category==null){
            return NotFound();
        }
        return category;
    }
    //POST: api/Category
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        string? user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid user_guid = new Guid(user_id);
        if (_context.Users.Any(u => u.UserId == user_guid))
        {
            User? user_action = _context.Users.FirstOrDefault(u => u.UserId == user_guid);
            eventManager.AddCategoryEvent("Add", category, user_action, _context);
        }

        return CreatedAtAction(nameof(GetCategory),new{id=category.CategoryId}, category);
    }

    //PUT:api/Category/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Category>> PutCategory(long id, Category category)
    {
        if(id != category.CategoryId)
        {
            return BadRequest();
        }

        _context.Entry(category).State=EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException)
        {
            if(!CategoryExists(id))
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
            eventManager.AddCategoryEvent("Edit", category, user_action, _context);
        }

        return Ok(category);
    }
    //DELETE: api/Todo/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(long id)
    {
        var category=await _context.Categories.FindAsync(id);
        if(category==null)
        {
            return NotFound();
        }
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        string? user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid user_guid = new Guid(user_id);
        if (_context.Users.Any(u => u.UserId == user_guid))
        {
            User? user_action = _context.Users.FirstOrDefault(u => u.UserId == user_guid);
            eventManager.AddCategoryEvent("Delete", category, user_action, _context);
        }

        return NoContent();
    }
    private bool CategoryExists(long id)
    {
        return _context.Categories.Any(e => e.CategoryId == id);
    }
}