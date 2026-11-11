using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;
using WebApplication1.Helpers;

namespace WebApplication1.Database.Configurations
{
    public class SpecialityConfiguration : IEntityTypeConfiguration<Speciality>
    {
        private const string TableName = "cd_speciality";

        public void Configure(EntityTypeBuilder<Speciality> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(p => p.SpecialityId)
                   .HasName($"pk_{TableName}_speciality_id");

            builder.Property(p => p.SpecialityId)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("speciality_id")
                   .HasComment("Идентификатор специальности");

            builder.Property(p => p.Title)
                   .IsRequired()
                   .HasColumnName("c_speciality_title")
                   .HasColumnType(ColumnType.String).HasMaxLength(200)
                   .HasComment("Название специальности");

            builder.Property(p => p.Code)
                   .IsRequired()
                   .HasColumnName("c_speciality_code")
                   .HasColumnType(ColumnType.String).HasMaxLength(50)
                   .HasComment("Код специальности (например, 09.02.07)");
        }
    }
}
