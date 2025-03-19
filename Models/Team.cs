using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DevHouse1.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        // Navigation property for projects
        //one to many 
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        // Navigation property for developers
        //one to many 
        public ICollection<Developer> Developers { get; set; } = new List<Developer>();
    }
}
