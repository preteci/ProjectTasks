using System.ComponentModel.DataAnnotations;
using DataTask = DAL.Entities.Task;
namespace DAL.Entities
{
    public class Project
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Code { get; set; }
        public List<DataTask>? Tasks { get; set; }
        [Required]
        public ProjectStatus Status { get; set; }
    }
    public enum ProjectStatus
    {
        NotStarted,
        InProgress,
        Completed,
        OnHold
    }
}
