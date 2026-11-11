using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;
using WebApplication1.Helpers;

namespace WebApplication1.Database.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        private const string TableName = "cd_group";

        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(p => p.GroupId)
                   .HasName($"pk_{TableName}_group_id");

            builder.Property(p => p.GroupId)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("group_id")
                   .HasComment("Идентификатор группы");

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasColumnName("c_group_name")
                   .HasColumnType(ColumnType.String).HasMaxLength(100)
                   .HasComment("Название группы");

            builder.Property(p => p.Course)
                   .IsRequired()
                   .HasColumnName("i_group_course")
                   .HasColumnType(ColumnType.Int)
                   .HasComment("Номер курса");

            builder.Property(p => p.IsDeleted)
                   .IsRequired()
                   .HasColumnName("b_group_is_deleted")
                   .HasColumnType(ColumnType.Bool)
                   .HasDefaultValue(false);

            // Связь: У Специальности много Групп
            builder.HasOne(p => p.Speciality)
                   .WithMany(g => g.Groups)
                   .HasForeignKey(p => p.SpecialityId)
                   .HasConstraintName("fk_f_speciality_id")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.SpecialityId, $"idx_{TableName}_fk_f_speciality_id");
        }
    }
}
