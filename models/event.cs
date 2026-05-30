using System.Text.Json.Serialization;
namespace nomination_api.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Event 
{
    // Unique GUID, Not Writeable but Readable
    [ScaffoldColumn(false)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
    public Guid EventId{get;set;} = Guid.NewGuid();
    public string? EventAction{get;set;}
    public string? EventMessage{get;set;}
    public Guid UserId{get;set;}
    [JsonIgnore]
    public User? User {get;set;}
    public DateTime EventDate{get;set;} = DateTime.Now; 
}