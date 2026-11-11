using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;
using WebApplication1.Helpers;

namespace WebApplication1.Database.Configurations
{
	public class StudentConfiguration : IEntityTypeConfiguration<Student>
	{
		private const string TableName = "cd_student";

		public void Configure(EntityTypeBuilder<Student> builder)
		{
			// Задаем первичный ключ
			builder.HasKey(p => p.StudentId)
				   .HasName($"pk_{TableName}_student_id");

			// Автогенерация ID (Identity)
			builder.Property(p => p.StudentId)
				   .ValueGeneratedOnAdd();

			// Настройка колонки ID
			builder.Property(p => p.StudentId)
				   .HasColumnName("student_id")
				   .HasComment("Идентификатор записи студента");

			// Имя студента
			builder.Property(p => p.FirstName)
				   .IsRequired()
				   .HasColumnName("c_student_firstname")
				   .HasColumnType(ColumnType.String).HasMaxLength(100)
				   .HasComment("Имя студента");

			// Фамилия студента
			builder.Property(p => p.LastName)
				   .IsRequired()
				   .HasColumnName("c_student_lastname")
				   .HasColumnType(ColumnType.String).HasMaxLength(100)
				   .HasComment("Фамилия студента");

			// Отчество студента (если нужно, можно добавить настройку для MiddleName)

			// Настройка связи "Один ко многим" (У студента одна группа, у группы много студентов)
			builder.ToTable(TableName);

			builder.HasOne(p => p.Group)
				   .WithMany()
				   .HasForeignKey(p => p.GroupId)
				   .HasConstraintName("fk_f_group_id")
				   .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление

			// Создание индекса по внешнему ключу для ускорения поиска
			builder.HasIndex(p => p.GroupId, $"idx_{TableName}_fk_f_group_id");

			// Явная автоподгрузка связанной сущности (Group) при выборке студентов
			builder.Navigation(p => p.Group)
				   .AutoInclude();
		}
	}
}