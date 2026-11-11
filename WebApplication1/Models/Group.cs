using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApplication1.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        [Required]
        public string Name { get; set; }
        public int Course {  get; set; }
        public int SpecialityId { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey(nameof(SpecialityId))]
        public Speciality? Speciality { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
