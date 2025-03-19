using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DevHouse1.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        // Foreign key for ProjectType
        [ForeignKey("ProjectType")]
        public int ProjectTypeId { get; set; }
        //many to one 
        [JsonIgnore]
        public ProjectType? ProjectType { get; set; }

        // Foreign key for Team
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        //navigation
        [JsonIgnore]
        public Team? Team { get; set; }
    }
}
