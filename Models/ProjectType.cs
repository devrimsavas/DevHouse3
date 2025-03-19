using System.ComponentModel.DataAnnotations;

namespace DevHouse1.Models 
{
    public class ProjectType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        //navigation 
        //one to many 
        public ICollection<Project> Projects {get;set;}=new List<Project>();
    }



}
