using System.ComponentModel.DataAnnotations;

namespace DevHouse1.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        // Navigation property
        //one to many 
        public ICollection<Developer> Developers { get; set; } = new List<Developer>();
    }
}
