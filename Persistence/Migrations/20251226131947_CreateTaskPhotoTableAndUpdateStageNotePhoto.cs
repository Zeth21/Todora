using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateTaskPhotoTableAndUpdateStageNotePhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StageNotePhotos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PhotoUploadDate",
                table: "StageNotePhotos",
                type: "datetime2(0)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "StageNotePhotos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TaskPhotos",
                columns: table => new
                {
                    PhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoUploadDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskPhotos", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_TaskPhotos_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskPhotos_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StageNotePhotos_UserId",
                table: "StageNotePhotos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskPhotos_TaskId",
                table: "TaskPhotos",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskPhotos_UserId",
                table: "TaskPhotos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StageNotePhotos_AspNetUsers_UserId",
                table: "StageNotePhotos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StageNotePhotos_AspNetUsers_UserId",
                table: "StageNotePhotos");

            migrationBuilder.DropTable(
                name: "TaskPhotos");

            migrationBuilder.DropIndex(
                name: "IX_StageNotePhotos_UserId",
                table: "StageNotePhotos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StageNotePhotos");

            migrationBuilder.DropColumn(
                name: "PhotoUploadDate",
                table: "StageNotePhotos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StageNotePhotos");
        }
    }
}
