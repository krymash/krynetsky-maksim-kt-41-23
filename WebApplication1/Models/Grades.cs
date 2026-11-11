using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Grade
    {
        [Key]
        public int GradeId {  get; set; }
        public int Value { get; set; }
        public int StudentId { get; set; }
        public int DisciplineId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student? Student { get; set; }

        [ForeignKey(nameof(DisciplineId))]
        public Discipline? Discipline { get; set; }
    }
}
