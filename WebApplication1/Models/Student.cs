using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApplication1.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GroupId { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group? Group { get; set; }
        public ICollection<Grade> Grades { get; set; }

    }

}
