using System.Text.Json.Serialization;
namespace nomination_api.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Role 
{
    [ScaffoldColumn(false)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid RoleId{get;set;}
    public required string RoleName{get;set;}
    [JsonIgnore]
    public ICollection<User> UsersInRole {get; set;} = new List<User>();
}