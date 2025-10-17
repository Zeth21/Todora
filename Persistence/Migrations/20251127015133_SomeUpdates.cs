using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SomeUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskRole",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "TaskCurrentStageId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskRoleId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Repositories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StageNotePhotos",
                columns: table => new
                {
                    PhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageNoteId = table.Column<int>(type: "int", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageNotePhotos", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_StageNotePhotos_StageNotes_StageNoteId",
                        column: x => x.StageNoteId,
                        principalTable: "StageNotes",
                        principalColumn: "StageNoteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskCurrentStageId",
                table: "Tasks",
                column: "TaskCurrentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskRoleId",
                table: "Tasks",
                column: "TaskRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_StageNotePhotos_StageNoteId",
                table: "StageNotePhotos",
                column: "StageNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Rolles_TaskRoleId",
                table: "Tasks",
                column: "TaskRoleId",
                principalTable: "Rolles",
                principalColumn: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskStages_TaskCurrentStageId",
                table: "Tasks",
                column: "TaskCurrentStageId",
                principalTable: "TaskStages",
                principalColumn: "TaskStageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Rolles_TaskRoleId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskStages_TaskCurrentStageId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "StageNotePhotos");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskCurrentStageId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskRoleId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskCurrentStageId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskRoleId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Repositories");

            migrationBuilder.AddColumn<int>(
                name: "TaskRole",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
