using System.Text.Json.Serialization;
namespace nomination_api.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Category
{
    [ScaffoldColumn(false)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long CategoryId{get;set;}
    public string? CategoryName {get;set;}
    [JsonIgnore]
    public ICollection<Nomination> NominationsInCategory {get; set;} = new List<Nomination>();
}