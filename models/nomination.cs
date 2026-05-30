using System.Text.Json.Serialization;
namespace nomination_api.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Nomination 
{
    [ScaffoldColumn(false)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
    public Guid NominationId{get;set;} = Guid.NewGuid();
    public Boolean Formal{get;set;}
    public string NominationMessage{get;set;} = ""; 
    public Guid NominatorId {get;set;}
    [JsonIgnore] // Navigation Properties we can Jsonignore they aren't real DB Columna however we can use them to navigate to User directly
    public User? Nominator {get;set;}
    public Guid NominatedId {get;set;}
    [JsonIgnore] // Navigation Properties we can Jsonignore they aren't real DB Columna however we can use them to navigate to User directly
    public User? Nominated {get;set;}
    public long CategoryId {get;set;}
    [JsonIgnore]
    public Category? Category {get;set;}
    public DateTime NominationDate{get;set;} = DateTime.Now; 
    [JsonIgnore]
    public DateTime NominationLastModified{get;set;} = DateTime.Now; 
}