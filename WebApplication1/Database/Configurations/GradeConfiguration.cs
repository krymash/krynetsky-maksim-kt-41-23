using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;
using WebApplication1.Helpers;

namespace WebApplication1.Database.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        private const string TableName = "cd_grade";

        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(p => p.GradeId)
                   .HasName($"pk_{TableName}_grade_id");

            builder.Property(p => p.GradeId)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("grade_id")
                   .HasComment("Идентификатор оценки");

            builder.Property(p => p.Value)
                   .IsRequired()
                   .HasColumnName("i_grade_value")
                   .HasColumnType(ColumnType.Int)
                   .HasComment("Значение оценки (например, 2, 3, 4, 5)");

            // Связь со Студентом
            builder.HasOne(p => p.Student)
                   .WithMany(s => s.Grades)
                   .HasForeignKey(p => p.StudentId)
                   .HasConstraintName("fk_f_student_id")
                   .OnDelete(DeleteBehavior.Cascade);

            // Связь с Дисциплиной
            builder.HasOne(p => p.Discipline)
                   .WithMany(d => d.Grades)
                   .HasForeignKey(p => p.DisciplineId)
                   .HasConstraintName("fk_f_discipline_id")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.StudentId, $"idx_{TableName}_fk_f_student_id");
            builder.HasIndex(p => p.DisciplineId, $"idx_{TableName}_fk_f_discipline_id");
        }
    }
}
