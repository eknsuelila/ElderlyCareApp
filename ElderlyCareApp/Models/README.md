# ElderlyCareApp Data Models Documentation

This document describes the main data models used in the ElderlyCareApp ASP.NET Core MVC project. These models represent the core entities for logging and monitoring elderly care activities.

---

## 1. User
Represents a family member or caregiver who interacts with the system.

**Properties:**
- `Id` (int): Unique identifier for the user.
- `Name` (string): Full name of the user.
- `Email` (string): Email address of the user.
- `Role` (UserRole enum): Role of the user, either `Family` or `Caregiver`.

---

## 2. ElderlyPerson
Represents an elderly individual being cared for.

**Properties:**
- `Id` (int): Unique identifier for the elderly person.
- `Name` (string): Full name of the elderly person.
- `DateOfBirth` (DateTime): Date of birth.
- `Notes` (string): Additional notes or medical information.

---

## 3. ActivityLog
Logs general daily activities for an elderly person.

**Properties:**
- `Id` (int): Unique identifier for the activity log entry.
- `ElderlyPersonId` (int): Foreign key to `ElderlyPerson`.
- `ElderlyPerson` (ElderlyPerson): Navigation property.
- `UserId` (int): Foreign key to `User` (who logged the activity).
- `User` (User): Navigation property.
- `ActivityType` (string): Type of activity (e.g., Exercise, Walk, Reading).
- `Description` (string): Additional details about the activity.
- `Timestamp` (DateTime): When the activity occurred.

---

## 4. MedicationLog
Logs medication intake for an elderly person.

**Properties:**
- `Id` (int): Unique identifier for the medication log entry.
- `ElderlyPersonId` (int): Foreign key to `ElderlyPerson`.
- `ElderlyPerson` (ElderlyPerson): Navigation property.
- `UserId` (int): Foreign key to `User` (who logged the medication).
- `User` (User): Navigation property.
- `MedicationName` (string): Name of the medication.
- `Dosage` (string): Dosage information.
- `Taken` (bool): Whether the medication was taken.
- `Timestamp` (DateTime): When the medication was (supposed to be) taken.

---

## 5. MealLog
Logs meal information for an elderly person.

**Properties:**
- `Id` (int): Unique identifier for the meal log entry.
- `ElderlyPersonId` (int): Foreign key to `ElderlyPerson`.
- `ElderlyPerson` (ElderlyPerson): Navigation property.
- `UserId` (int): Foreign key to `User` (who logged the meal).
- `User` (User): Navigation property.
- `MealType` (string): Type of meal (e.g., Breakfast, Lunch, Dinner, Snack).
- `Description` (string): Additional details about the meal.
- `Timestamp` (DateTime): When the meal occurred.

---

## 6. SocialInteractionLog
Logs social interactions for an elderly person.

**Properties:**
- `Id` (int): Unique identifier for the social interaction log entry.
- `ElderlyPersonId` (int): Foreign key to `ElderlyPerson`.
- `ElderlyPerson` (ElderlyPerson): Navigation property.
- `UserId` (int): Foreign key to `User` (who logged the interaction).
- `User` (User): Navigation property.
- `InteractionType` (string): Type of interaction (e.g., Call, Visit).
- `Description` (string): Additional details about the interaction.
- `Timestamp` (DateTime): When the interaction occurred.

---

## Relationships
- Each log (Activity, Medication, Meal, Social Interaction) is linked to both an `ElderlyPerson` and a `User`.
- The `User` model uses an enum (`UserRole`) to distinguish between family members and caregivers.

---

For further details, see the individual model class files in this directory. 

---

# Database Setup Steps

Follow these steps to set up the database for ElderlyCareApp using SQL Server and Entity Framework Core:

1. **Install Entity Framework Core Packages**
   - Install the following NuGet packages:
     - Microsoft.EntityFrameworkCore
     - Microsoft.EntityFrameworkCore.SqlServer
     - Microsoft.EntityFrameworkCore.Tools

2. **Create the ApplicationDbContext**
   - Implement a class `ApplicationDbContext` in the `Models` folder, inheriting from `DbContext`.
   - Add `DbSet` properties for each model (User, ElderlyPerson, ActivityLog, MedicationLog, MealLog, SocialInteractionLog).

3. **Register DbContext in Program.cs**
   - In `Program.cs`, add the DbContext to the service container using the connection string from `appsettings.json`:
     ```csharp
     builder.Services.AddDbContext<ApplicationDbContext>(options =>
         options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
     ```

4. **Configure the Connection String**
   - In `appsettings.json`, add a connection string named `DefaultConnection`:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ElderlyCareDb;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
     ```

5. **Add and Run Migrations**
   - Open the Package Manager Console or terminal and run:
     ```sh
     dotnet ef migrations add InitialCreate
     dotnet ef database update
     ```
   - This will create the database schema based on your models.

6. **(Optional) Seed Initial Data**
   - You can add seed data in the `OnModelCreating` method of `ApplicationDbContext` or via a custom seeder.

Refer back to this section as you complete each step in your project setup. 

---

## Completed Steps

- Entity Framework Core packages are already installed in the project.
- The `ApplicationDbContext` class has been created in the `Models` folder, inheriting from `DbContext` and including DbSet properties for all models.
- The `ApplicationDbContext` has been registered in `Program.cs` using the following code:
  ```csharp
  builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
  ```
- The connection string named `DefaultConnection` is present in `appsettings.json`:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ElderlyCareDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
  ```

Continue with migrations to create the database schema. 

- The initial migration was created using:
  ```sh
  dotnet ef migrations add InitialCreate --project ElderlyCareApp/ElderlyCareApp.csproj
  ```
- The migration was applied and the database schema was created using:
  ```sh
  dotnet ef database update --project ElderlyCareApp/ElderlyCareApp.csproj
  ```
- The SQL Server database is now ready for use with the defined models. 