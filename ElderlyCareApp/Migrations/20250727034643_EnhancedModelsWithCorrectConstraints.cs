using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElderlyCareApp.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedModelsWithCorrectConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_Users_UserId",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MealLogs_Users_UserId",
                table: "MealLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationLogs_Users_UserId",
                table: "MedicationLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialInteractionLogs_Users_UserId",
                table: "SocialInteractionLogs");

            migrationBuilder.DropIndex(
                name: "IX_MedicationLogs_ElderlyPersonId",
                table: "MedicationLogs");

            migrationBuilder.DropIndex(
                name: "IX_MealLogs_ElderlyPersonId",
                table: "MealLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_ElderlyPersonId",
                table: "ActivityLogs");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "SocialInteractionLogs",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "MedicationLogs",
                newName: "ScheduledTime");

            migrationBuilder.RenameColumn(
                name: "Taken",
                table: "MedicationLogs",
                newName: "RequiresRefill");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "MealLogs",
                newName: "MealTime");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "ActivityLogs",
                newName: "StartTime");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactPhone",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactRelationship",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specializations",
                table: "Users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InteractionType",
                table: "SocialInteractionLogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SocialInteractionLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Activities",
                table: "SocialInteractionLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SocialInteractionLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "SocialInteractionLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "SocialInteractionLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EngagementLevel",
                table: "SocialInteractionLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InteractionTitle",
                table: "SocialInteractionLogs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "SocialInteractionLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "SocialInteractionLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MoodAfter",
                table: "SocialInteractionLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MoodBefore",
                table: "SocialInteractionLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "SocialInteractionLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Participants",
                table: "SocialInteractionLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quality",
                table: "SocialInteractionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Relationship",
                table: "SocialInteractionLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TopicsDiscussed",
                table: "SocialInteractionLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SocialInteractionLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MedicationName",
                table: "MedicationLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Dosage",
                table: "MedicationLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualTime",
                table: "MedicationLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MedicationLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "MedicationLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Frequency",
                table: "MedicationLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "MedicationLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Interactions",
                table: "MedicationLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "MedicationLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pharmacy",
                table: "MedicationLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PharmacyPhone",
                table: "MedicationLogs",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrescribedBy",
                table: "MedicationLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PrescriptionDate",
                table: "MedicationLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefillReminderDate",
                table: "MedicationLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Route",
                table: "MedicationLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SideEffects",
                table: "MedicationLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MedicationLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MedicationLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MealType",
                table: "MealLogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MealLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AllergiesNoted",
                table: "MealLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppetiteLevel",
                table: "MealLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Beverages",
                table: "MealLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MealLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DietaryCompliance",
                table: "MealLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "MealLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Foods",
                table: "MealLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "MealLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MealName",
                table: "MealLogs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "MealLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PercentageEaten",
                table: "MealLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quality",
                table: "MealLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Supplements",
                table: "MealLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MealLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "ElderlyPeople",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ElderlyPeople",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "ElderlyPeople",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "ElderlyPeople",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "ElderlyPeople",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarePreferences",
                table: "ElderlyPeople",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ElderlyPeople",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ElderlyPeople",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DietaryRestrictions",
                table: "ElderlyPeople",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "ElderlyPeople",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactPhone",
                table: "ElderlyPeople",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactRelationship",
                table: "ElderlyPeople",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsurancePolicyNumber",
                table: "ElderlyPeople",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceProvider",
                table: "ElderlyPeople",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ElderlyPeople",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MedicalConditions",
                table: "ElderlyPeople",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobilityAids",
                table: "ElderlyPeople",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ElderlyPeople",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhysicianPhone",
                table: "ElderlyPeople",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "ElderlyPeople",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryCarePhysician",
                table: "ElderlyPeople",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ElderlyPeople",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ActivityLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ActivityType",
                table: "ActivityLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "ActivityLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ActivityLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "ActivityLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "ActivityLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnergyLevel",
                table: "ActivityLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnjoymentLevel",
                table: "ActivityLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "ActivityLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ActivityLogs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "ActivityLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ActivityLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppointmentLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElderlyPersonId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AppointmentType = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProviderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProviderSpecialty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ScheduledDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Symptoms = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Diagnosis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Treatment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Prescriptions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Recommendations = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FollowUpInstructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FollowUpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequiresTransportation = table.Column<bool>(type: "bit", nullable: false),
                    TransportationNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentLogs_ElderlyPeople_ElderlyPersonId",
                        column: x => x.ElderlyPersonId,
                        principalTable: "ElderlyPeople",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CarePlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElderlyPersonId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Goals = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Objectives = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Interventions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SuccessCriteria = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RiskFactors = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Precautions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Equipment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Resources = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurrencePattern = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProgressPercentage = table.Column<int>(type: "int", nullable: true),
                    ProgressNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Barriers = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Solutions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarePlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarePlans_ElderlyPeople_ElderlyPersonId",
                        column: x => x.ElderlyPersonId,
                        principalTable: "ElderlyPeople",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarePlans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SymptomLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElderlyPersonId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LogTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Symptom = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    PainLevel = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Triggers = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReliefMethods = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsOngoing = table.Column<bool>(type: "bit", nullable: false),
                    ResolvedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Mood = table.Column<int>(type: "int", nullable: false),
                    AnxietyLevel = table.Column<int>(type: "int", nullable: true),
                    DepressionLevel = table.Column<int>(type: "int", nullable: true),
                    SleepQuality = table.Column<int>(type: "int", nullable: true),
                    HoursSlept = table.Column<int>(type: "int", nullable: true),
                    CognitiveSymptoms = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MemoryIssues = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Confusion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhysicalSymptoms = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MobilityIssues = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BalanceIssues = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DigestiveIssues = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RespiratoryIssues = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CardiovascularIssues = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MedicationSideEffects = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MedicationCompliance = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequiresMedicalAttention = table.Column<bool>(type: "bit", nullable: false),
                    MedicalActionTaken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomLogs_ElderlyPeople_ElderlyPersonId",
                        column: x => x.ElderlyPersonId,
                        principalTable: "ElderlyPeople",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SymptomLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VitalSignsLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElderlyPersonId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MeasurementTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SystolicPressure = table.Column<int>(type: "int", nullable: true),
                    DiastolicPressure = table.Column<int>(type: "int", nullable: true),
                    HeartRate = table.Column<int>(type: "int", nullable: true),
                    Temperature = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OxygenSaturation = table.Column<int>(type: "int", nullable: true),
                    RespiratoryRate = table.Column<int>(type: "int", nullable: true),
                    BloodSugar = table.Column<int>(type: "int", nullable: true),
                    PainLevel = table.Column<int>(type: "int", nullable: true),
                    MeasurementLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MeasurementMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsNormal = table.Column<bool>(type: "bit", nullable: false),
                    Abnormalities = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalSignsLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VitalSignsLogs_ElderlyPeople_ElderlyPersonId",
                        column: x => x.ElderlyPersonId,
                        principalTable: "ElderlyPeople",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VitalSignsLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElderlyPersonId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CarePlanId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TaskType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    ScheduledTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Instructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Requirements = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurrenceType = table.Column<int>(type: "int", nullable: false),
                    RecurrenceInterval = table.Column<int>(type: "int", nullable: true),
                    RecurrenceDays = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RecurrenceEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaxOccurrences = table.Column<int>(type: "int", nullable: true),
                    HasReminder = table.Column<bool>(type: "bit", nullable: false),
                    ReminderMinutesBefore = table.Column<int>(type: "int", nullable: true),
                    LastReminderSent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletionNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CompletionRating = table.Column<int>(type: "int", nullable: true),
                    Barriers = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Solutions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RelatedMedication = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RelatedAppointment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RelatedActivity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskSchedules_CarePlans_CarePlanId",
                        column: x => x.CarePlanId,
                        principalTable: "CarePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TaskSchedules_ElderlyPeople_ElderlyPersonId",
                        column: x => x.ElderlyPersonId,
                        principalTable: "ElderlyPeople",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskSchedules_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicationLogs_ElderlyPersonId_ScheduledTime",
                table: "MedicationLogs",
                columns: new[] { "ElderlyPersonId", "ScheduledTime" });

            migrationBuilder.CreateIndex(
                name: "IX_MealLogs_ElderlyPersonId_MealTime",
                table: "MealLogs",
                columns: new[] { "ElderlyPersonId", "MealTime" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_ElderlyPersonId_StartTime",
                table: "ActivityLogs",
                columns: new[] { "ElderlyPersonId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentLogs_ElderlyPersonId_ScheduledDateTime",
                table: "AppointmentLogs",
                columns: new[] { "ElderlyPersonId", "ScheduledDateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentLogs_UserId",
                table: "AppointmentLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarePlans_ElderlyPersonId",
                table: "CarePlans",
                column: "ElderlyPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CarePlans_UserId",
                table: "CarePlans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomLogs_ElderlyPersonId",
                table: "SymptomLogs",
                column: "ElderlyPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomLogs_UserId",
                table: "SymptomLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSchedules_CarePlanId",
                table: "TaskSchedules",
                column: "CarePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSchedules_ElderlyPersonId_ScheduledTime",
                table: "TaskSchedules",
                columns: new[] { "ElderlyPersonId", "ScheduledTime" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskSchedules_Status_ScheduledTime",
                table: "TaskSchedules",
                columns: new[] { "Status", "ScheduledTime" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskSchedules_UserId",
                table: "TaskSchedules",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VitalSignsLogs_ElderlyPersonId_MeasurementTime",
                table: "VitalSignsLogs",
                columns: new[] { "ElderlyPersonId", "MeasurementTime" });

            migrationBuilder.CreateIndex(
                name: "IX_VitalSignsLogs_UserId",
                table: "VitalSignsLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_Users_UserId",
                table: "ActivityLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MealLogs_Users_UserId",
                table: "MealLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationLogs_Users_UserId",
                table: "MedicationLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialInteractionLogs_Users_UserId",
                table: "SocialInteractionLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_Users_UserId",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MealLogs_Users_UserId",
                table: "MealLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationLogs_Users_UserId",
                table: "MedicationLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialInteractionLogs_Users_UserId",
                table: "SocialInteractionLogs");

            migrationBuilder.DropTable(
                name: "AppointmentLogs");

            migrationBuilder.DropTable(
                name: "SymptomLogs");

            migrationBuilder.DropTable(
                name: "TaskSchedules");

            migrationBuilder.DropTable(
                name: "VitalSignsLogs");

            migrationBuilder.DropTable(
                name: "CarePlans");

            migrationBuilder.DropIndex(
                name: "IX_MedicationLogs_ElderlyPersonId_ScheduledTime",
                table: "MedicationLogs");

            migrationBuilder.DropIndex(
                name: "IX_MealLogs_ElderlyPersonId_MealTime",
                table: "MealLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_ElderlyPersonId_StartTime",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmergencyContactPhone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmergencyContactRelationship",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLoginAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Specializations",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Activities",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "EngagementLevel",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "InteractionTitle",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "MoodAfter",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "MoodBefore",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "Participants",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "Quality",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "Relationship",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "TopicsDiscussed",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SocialInteractionLogs");

            migrationBuilder.DropColumn(
                name: "ActualTime",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "Interactions",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "Pharmacy",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "PharmacyPhone",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "PrescribedBy",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "PrescriptionDate",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "RefillReminderDate",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "Route",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "SideEffects",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "AllergiesNoted",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "AppetiteLevel",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "Beverages",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "DietaryCompliance",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "Foods",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "MealName",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "PercentageEaten",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "Quality",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "Supplements",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MealLogs");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "CarePreferences",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "City",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "DietaryRestrictions",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "EmergencyContactPhone",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "EmergencyContactRelationship",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "InsurancePolicyNumber",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "InsuranceProvider",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "MedicalConditions",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "MobilityAids",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "PhysicianPhone",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "PrimaryCarePhysician",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ElderlyPeople");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "EnergyLevel",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "EnjoymentLevel",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ActivityLogs");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "SocialInteractionLogs",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "ScheduledTime",
                table: "MedicationLogs",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "RequiresRefill",
                table: "MedicationLogs",
                newName: "Taken");

            migrationBuilder.RenameColumn(
                name: "MealTime",
                table: "MealLogs",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "ActivityLogs",
                newName: "Timestamp");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "InteractionType",
                table: "SocialInteractionLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SocialInteractionLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MedicationName",
                table: "MedicationLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Dosage",
                table: "MedicationLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MealType",
                table: "MealLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MealLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "ElderlyPeople",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ElderlyPeople",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ActivityType",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_MedicationLogs_ElderlyPersonId",
                table: "MedicationLogs",
                column: "ElderlyPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_MealLogs_ElderlyPersonId",
                table: "MealLogs",
                column: "ElderlyPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_ElderlyPersonId",
                table: "ActivityLogs",
                column: "ElderlyPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_Users_UserId",
                table: "ActivityLogs",
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

            migrationBuilder.AddForeignKey(
                name: "FK_SocialInteractionLogs_Users_UserId",
                table: "SocialInteractionLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
