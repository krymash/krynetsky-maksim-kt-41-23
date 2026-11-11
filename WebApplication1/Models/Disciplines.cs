using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Discipline
    {
        [Key]
        public int DisciplineId { get; set; }

        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Grade> Grades { get; set; }
    }

    }
