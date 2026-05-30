using nomination_api.models;
using nomination_api.DataBaseContext;

namespace nomination_api.internal_methods; 
public class EventManager
{
    public EventManager()
    {
        
    }
    public void AddUserEvent(string Action, User user_added, User user_action, DatabaseContext _context)
    {
        var eventaction = new Event {
        EventMessage = $"User {user_action.UserName} has created the new user {user_added.UserName}",
        EventAction = $"{Action}",
        UserId = user_action.UserId
        };
        PublishEvents(_context, eventaction);
    }

    public void AddNominationEvent(string Action, Nomination nomination, DatabaseContext _context)
    {
        var eventaction = new Event
        {
            EventMessage = $"{nomination.Nominator.UserName} has nominated {nomination.Nominated.UserName}",
            EventAction = $"{Action}",
            UserId = nomination.NominatorId
        };
        PublishEvents(_context, eventaction);
    }

    public void AdminLoginEvent (string Action, User user_login, DatabaseContext _context)
    {
        var eventaction = new Event
        {
            EventMessage = $"{user_login.UserName} logged into admin portal",
            EventAction = $"{Action}",
            UserId = user_login.UserId
        };
        PublishEvents(_context, eventaction);
    }

    public void AddCategoryEvent(string Action, Category category, User user_action, DatabaseContext _context)
    {
        var eventaction = new Event
        {
            EventMessage = $"User {user_action.UserName} has {Action}d the category {category.CategoryName}",
            EventAction = $"{Action}",
            UserId = user_action.UserId
        };
        PublishEvents(_context, eventaction);
    }

    public void AddRoleEvent(string Action, Role role, User user_action, DatabaseContext _context)
    {
        var eventaction = new Event
        {
            EventMessage = $"User {user_action.UserName} has {Action}d the role {role.RoleName}",
            EventAction = $"{Action}",
            UserId = user_action.UserId
        };
        PublishEvents(_context, eventaction);
    }

    public void PublishEvents(DatabaseContext _context, Event eventaction)
    {
        _context.Events.Add(eventaction);
        _context.SaveChanges();
    }

}