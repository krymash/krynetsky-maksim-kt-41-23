using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;
using WebApplication1.Helpers;

namespace WebApplication1.Database.Configurations
{
    public class DisciplineConfiguration : IEntityTypeConfiguration<Discipline>
    {
        // Название таблицы в базе данных в нижнем регистре
        private const string TableName = "cd_discipline";

        public void Configure(EntityTypeBuilder<Discipline> builder)
        {
            // Указываем имя таблицы
            builder.ToTable(TableName);

            // Задаем первичный ключ и его имя ограничения в БД
            builder.HasKey(p => p.DisciplineId)
                   .HasName($"pk_{TableName}_discipline_id");

            // Настраиваем автогенерацию ID (Identity) и имя колонки
            builder.Property(p => p.DisciplineId)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("discipline_id")
                   .HasComment("Идентификатор дисциплины");

            // Настройка колонки Name (Название дисциплины)
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasColumnName("c_discipline_name")
                   // Используем созданный ранее хелпер ColumnType
                   .HasColumnType(ColumnType.String).HasMaxLength(150)
                   .HasComment("Название дисциплины");

            // Настройка колонки IsDeleted (Флаг мягкого удаления)
            builder.Property(p => p.IsDeleted)
                   .IsRequired()
                   .HasColumnName("b_discipline_is_deleted")
                   .HasColumnType(ColumnType.Bool)
                   .HasDefaultValue(false) // По умолчанию дисциплина активна
                   .HasComment("Флаг мягкого удаления дисциплины");
        }
    }
}
