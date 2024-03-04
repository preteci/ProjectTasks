using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Task
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
        public int? ProjectId {get; set; }
        [JsonIgnore]
        public Project? Project { get; set; }
    }
}
