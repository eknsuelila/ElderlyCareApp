using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElderlyCareApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCaregiverAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_Users_UserId",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentLogs_Users_UserId",
                table: "AppointmentLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_CareNotes_Users_UserId",
                table: "CareNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_MealLogs_Users_UserId",
                table: "MealLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationLogs_Users_UserId",
                table: "MedicationLogs");

            migrationBuilder.AddColumn<int>(
                name: "ElderlyPersonId1",
                table: "MedicationLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ElderlyPersonId1",
                table: "MealLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ElderlyPersonId1",
                table: "CareNotes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ElderlyPersonId1",
                table: "AppointmentLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ElderlyPersonId1",
                table: "ActivityLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CaregiverAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaregiverId = table.Column<int>(type: "int", nullable: false),
                    ElderlyPersonId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaregiverAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaregiverAssignments_ElderlyPeople_ElderlyPersonId",
                        column: x => x.ElderlyPersonId,
                        principalTable: "ElderlyPeople",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaregiverAssignments_Users_CaregiverId",
                        column: x => x.CaregiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicationLogs_ElderlyPersonId1",
                table: "MedicationLogs",
                column: "ElderlyPersonId1");

            migrationBuilder.CreateIndex(
                name: "IX_MealLogs_ElderlyPersonId1",
                table: "MealLogs",
                column: "ElderlyPersonId1");

            migrationBuilder.CreateIndex(
                name: "IX_CareNotes_ElderlyPersonId1",
                table: "CareNotes",
                column: "ElderlyPersonId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentLogs_ElderlyPersonId1",
                table: "AppointmentLogs",
                column: "ElderlyPersonId1");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_ElderlyPersonId1",
                table: "ActivityLogs",
                column: "ElderlyPersonId1");

            migrationBuilder.CreateIndex(
                name: "IX_CaregiverAssignments_CaregiverId_ElderlyPersonId_IsActive",
                table: "CaregiverAssignments",
                columns: new[] { "CaregiverId", "ElderlyPersonId", "IsActive" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_CaregiverAssignments_ElderlyPersonId",
                table: "CaregiverAssignments",
                column: "ElderlyPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_ElderlyPeople_ElderlyPersonId1",
                table: "ActivityLogs",
                column: "ElderlyPersonId1",
                principalTable: "ElderlyPeople",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_Users_UserId",
                table: "ActivityLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentLogs_ElderlyPeople_ElderlyPersonId1",
                table: "AppointmentLogs",
                column: "ElderlyPersonId1",
                principalTable: "ElderlyPeople",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentLogs_Users_UserId",
                table: "AppointmentLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CareNotes_ElderlyPeople_ElderlyPersonId1",
                table: "CareNotes",
                column: "ElderlyPersonId1",
                principalTable: "ElderlyPeople",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CareNotes_Users_UserId",
                table: "CareNotes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MealLogs_ElderlyPeople_ElderlyPersonId1",
                table: "MealLogs",
                column: "ElderlyPersonId1",
                principalTable: "ElderlyPeople",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealLogs_Users_UserId",
                table: "MealLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationLogs_ElderlyPeople_ElderlyPersonId1",
                table: "MedicationLogs",
                column: "ElderlyPersonId1",
                principalTable: "ElderlyPeople",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationLogs_Users_UserId",
                table: "MedicationLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_ElderlyPeople_ElderlyPersonId1",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_Users_UserId",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentLogs_ElderlyPeople_ElderlyPersonId1",
                table: "AppointmentLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentLogs_Users_UserId",
                table: "AppointmentLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_CareNotes_ElderlyPeople_ElderlyPersonId1",
                table: "CareNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_CareNotes_Users_UserId",
                table: "CareNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_MealLogs_ElderlyPeople_ElderlyPersonId1",
                table: "MealLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MealLogs_Users_UserId",
                table: "MealLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationLogs_ElderlyPeople_ElderlyPersonId1",
                table: "MedicationLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationLogs_Users_UserId",
                table: "MedicationLogs");

            migrationBuilder.DropTable(
                name: "CaregiverAssignments");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_MedicationLogs_ElderlyPersonId1",
                table: "MedicationLogs");

            migrationBuilder.DropIndex(
                name: "IX_MealLogs_ElderlyPersonId1",
                table: "MealLogs");

            migrationBuilder.DropIndex(
                name: "IX_CareNotes_ElderlyPersonId1",
                table: "CareNotes");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentLogs_ElderlyPersonId1",
                table: "AppointmentLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_ElderlyPersonId1",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "ElderlyPersonId1",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "ElderlyPersonId1",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "ElderlyPersonId1",
                table: "CareNotes");

            migrationBuilder.DropColumn(
                name: "ElderlyPersonId1",
                table: "AppointmentLogs");

            migrationBuilder.DropColumn(
                name: "ElderlyPersonId1",
                table: "ActivityLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_Users_UserId",
                table: "ActivityLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentLogs_Users_UserId",
                table: "AppointmentLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CareNotes_Users_UserId",
                table: "CareNotes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealLogs_Users_UserId",
                table: "MealLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationLogs_Users_UserId",
                table: "MedicationLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
