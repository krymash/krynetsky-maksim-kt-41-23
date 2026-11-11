using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class migr1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cd_discipline",
                columns: table => new
                {
                    discipline_id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор дисциплины")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    c_discipline_name = table.Column<string>(type: "varchar", maxLength: 150, nullable: false, comment: "Название дисциплины"),
                    b_discipline_is_deleted = table.Column<bool>(type: "bool", nullable: false, defaultValue: false, comment: "Флаг мягкого удаления дисциплины")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cd_discipline_discipline_id", x => x.discipline_id);
                });

            migrationBuilder.CreateTable(
                name: "cd_speciality",
                columns: table => new
                {
                    speciality_id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор специальности")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    c_speciality_title = table.Column<string>(type: "varchar", maxLength: 200, nullable: false, comment: "Название специальности"),
                    c_speciality_code = table.Column<string>(type: "varchar", maxLength: 50, nullable: false, comment: "Код специальности (например, 09.02.07)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cd_speciality_speciality_id", x => x.speciality_id);
                });

            migrationBuilder.CreateTable(
                name: "cd_group",
                columns: table => new
                {
                    group_id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор группы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    c_group_name = table.Column<string>(type: "varchar", maxLength: 100, nullable: false, comment: "Название группы"),
                    i_group_course = table.Column<int>(type: "int4", nullable: false, comment: "Номер курса"),
                    SpecialityId = table.Column<int>(type: "integer", nullable: false),
                    b_group_is_deleted = table.Column<bool>(type: "bool", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cd_group_group_id", x => x.group_id);
                    table.ForeignKey(
                        name: "fk_f_speciality_id",
                        column: x => x.SpecialityId,
                        principalTable: "cd_speciality",
                        principalColumn: "speciality_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cd_student",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор записи студента")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    c_student_firstname = table.Column<string>(type: "varchar", maxLength: 100, nullable: false, comment: "Имя студента"),
                    c_student_lastname = table.Column<string>(type: "varchar", maxLength: 100, nullable: false, comment: "Фамилия студента"),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    GroupId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cd_student_student_id", x => x.student_id);
                    table.ForeignKey(
                        name: "FK_cd_student_cd_group_GroupId1",
                        column: x => x.GroupId1,
                        principalTable: "cd_group",
                        principalColumn: "group_id");
                    table.ForeignKey(
                        name: "fk_f_group_id",
                        column: x => x.GroupId,
                        principalTable: "cd_group",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cd_grade",
                columns: table => new
                {
                    grade_id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор оценки")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    i_grade_value = table.Column<int>(type: "int4", nullable: false, comment: "Значение оценки (например, 2, 3, 4, 5)"),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    DisciplineId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cd_grade_grade_id", x => x.grade_id);
                    table.ForeignKey(
                        name: "fk_f_discipline_id",
                        column: x => x.DisciplineId,
                        principalTable: "cd_discipline",
                        principalColumn: "discipline_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_f_student_id",
                        column: x => x.StudentId,
                        principalTable: "cd_student",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_cd_grade_fk_f_discipline_id",
                table: "cd_grade",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "idx_cd_grade_fk_f_student_id",
                table: "cd_grade",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "idx_cd_group_fk_f_speciality_id",
                table: "cd_group",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "idx_cd_student_fk_f_group_id",
                table: "cd_student",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_cd_student_GroupId1",
                table: "cd_student",
                column: "GroupId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cd_grade");

            migrationBuilder.DropTable(
                name: "cd_discipline");

            migrationBuilder.DropTable(
                name: "cd_student");

            migrationBuilder.DropTable(
                name: "cd_group");

            migrationBuilder.DropTable(
                name: "cd_speciality");
        }
    }
}
