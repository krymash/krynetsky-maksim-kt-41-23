using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApplication1.Models
{
    public class Speciality
    {
        [Key]
        public int SpecialityId { get; set; }
        public string Title { get; set; }
        public string Code {  get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}