using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_RepositoryRoles_AspNetUsers_UserId",
                table: "RepositoryRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_RepositoryRoles_Repositories_RepositoryId",
                table: "RepositoryRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_RepositoryRoles_Rolles_RoleId",
                table: "RepositoryRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_StageNote_AspNetUsers_UserId",
                table: "StageNote");

            migrationBuilder.DropForeignKey(
                name: "FK_StageNote_TaskStages_TaskStageId",
                table: "StageNote");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Repositories_RepositoryId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskStages_Stages_StageId",
                table: "TaskStages");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskStages_Tasks_TaskId",
                table: "TaskStages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StageNote",
                table: "StageNote");

            migrationBuilder.RenameTable(
                name: "StageNote",
                newName: "StageNotes");

            migrationBuilder.RenameIndex(
                name: "IX_StageNote_UserId",
                table: "StageNotes",
                newName: "IX_StageNotes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_StageNote_TaskStageId",
                table: "StageNotes",
                newName: "IX_StageNotes_TaskStageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StageNotes",
                table: "StageNotes",
                column: "StageNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepositoryRoles_AspNetUsers_UserId",
                table: "RepositoryRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepositoryRoles_Repositories_RepositoryId",
                table: "RepositoryRoles",
                column: "RepositoryId",
                principalTable: "Repositories",
                principalColumn: "RepositoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepositoryRoles_Rolles_RoleId",
                table: "RepositoryRoles",
                column: "RoleId",
                principalTable: "Rolles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StageNotes_AspNetUsers_UserId",
                table: "StageNotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StageNotes_TaskStages_TaskStageId",
                table: "StageNotes",
                column: "TaskStageId",
                principalTable: "TaskStages",
                principalColumn: "TaskStageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Repositories_RepositoryId",
                table: "Tasks",
                column: "RepositoryId",
                principalTable: "Repositories",
                principalColumn: "RepositoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskStages_Stages_StageId",
                table: "TaskStages",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "StageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskStages_Tasks_TaskId",
                table: "TaskStages",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_RepositoryRoles_AspNetUsers_UserId",
                table: "RepositoryRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_RepositoryRoles_Repositories_RepositoryId",
                table: "RepositoryRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_RepositoryRoles_Rolles_RoleId",
                table: "RepositoryRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_StageNotes_AspNetUsers_UserId",
                table: "StageNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_StageNotes_TaskStages_TaskStageId",
                table: "StageNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Repositories_RepositoryId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskStages_Stages_StageId",
                table: "TaskStages");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskStages_Tasks_TaskId",
                table: "TaskStages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StageNotes",
                table: "StageNotes");

            migrationBuilder.RenameTable(
                name: "StageNotes",
                newName: "StageNote");

            migrationBuilder.RenameIndex(
                name: "IX_StageNotes_UserId",
                table: "StageNote",
                newName: "IX_StageNote_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_StageNotes_TaskStageId",
                table: "StageNote",
                newName: "IX_StageNote_TaskStageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StageNote",
                table: "StageNote",
                column: "StageNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepositoryRoles_AspNetUsers_UserId",
                table: "RepositoryRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RepositoryRoles_Repositories_RepositoryId",
                table: "RepositoryRoles",
                column: "RepositoryId",
                principalTable: "Repositories",
                principalColumn: "RepositoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepositoryRoles_Rolles_RoleId",
                table: "RepositoryRoles",
                column: "RoleId",
                principalTable: "Rolles",
                principalColumn: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_StageNote_AspNetUsers_UserId",
                table: "StageNote",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StageNote_TaskStages_TaskStageId",
                table: "StageNote",
                column: "TaskStageId",
                principalTable: "TaskStages",
                principalColumn: "TaskStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Repositories_RepositoryId",
                table: "Tasks",
                column: "RepositoryId",
                principalTable: "Repositories",
                principalColumn: "RepositoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskStages_Stages_StageId",
                table: "TaskStages",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskStages_Tasks_TaskId",
                table: "TaskStages",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId");
        }
    }
}
