using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace nomination_api.models;

[Index(nameof(UserName), IsUnique = true)]
public class User 
{
    [ScaffoldColumn(false)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
    public Guid UserId{get;set;} = Guid.NewGuid();
      [StringLength(16, MinimumLength = 5, ErrorMessage = "min 5, max 16 letters")] 
          public required string UserName{get;set;}
    public required string Email{get;set;}
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)] //Ensures password is not returned when doing GET requests, but still allows it to be POST
    public required string UserPassword {get;set;}
    public Guid RoleId {get;set;}
    [JsonIgnore]
    public Role? Role {get;set;}
    [JsonIgnore]
    public DateTime UserCreatedDate{get;set;} = DateTime.Now; 
    [JsonIgnore]
    public DateTime UserLastUpdated{get;set;} = DateTime.Now; 
    [JsonIgnore] // Not actual DB columns however we can use both to map nominations give/recieved per user
    public ICollection<Nomination> NominationsGiven {get; set;} = new List<Nomination>();
    [JsonIgnore]
    public ICollection<Nomination> NominationsReceived {get; set;} = new List<Nomination>();
    [JsonIgnore]
    public ICollection<Event> EventsGenerated {get; set;} = new List<Event>();
    
}
