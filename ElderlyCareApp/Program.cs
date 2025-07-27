using Microsoft.EntityFrameworkCore;
using ElderlyCareApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add session middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    SeedData(context);
}

app.Run();

// Seed data method
static void SeedData(ApplicationDbContext context)
{
    // Ensure demo users with passwords exist
    var demoEmails = new[] { "family@elderlycare.com", "caregiver@elderlycare.com", "maria@elderlycare.com", "david@elderlycare.com" };
    
    // Check if demo users exist
    var existingDemoUsers = context.Users.Where(u => demoEmails.Contains(u.Email)).ToList();
    
    // If demo users don't exist, create them
    if (!existingDemoUsers.Any())
    {
        var users = new List<User>
        {
            new User { 
                Name = "Sarah Johnson", 
                Email = "family@elderlycare.com", 
                Password = "password123",
                PhoneNumber = "555-0101", 
                Role = UserRole.Family, 
                Specializations = "Family Care" 
            },
            new User { 
                Name = "Michael Chen", 
                Email = "caregiver@elderlycare.com", 
                Password = "password123",
                PhoneNumber = "555-0102", 
                Role = UserRole.Caregiver, 
                Specializations = "Elderly Care" 
            },
            new User { 
                Name = "Maria Rodriguez", 
                Email = "maria@elderlycare.com", 
                Password = "password123",
                PhoneNumber = "555-0103", 
                Role = UserRole.Caregiver, 
                Specializations = "Daily Care" 
            },
            new User { 
                Name = "David Wilson", 
                Email = "david@elderlycare.com", 
                Password = "password123",
                PhoneNumber = "555-0104", 
                Role = UserRole.Caregiver, 
                Specializations = "Physical Therapy" 
            }
        };

        context.Users.AddRange(users);
        context.SaveChanges();
    }

    // Only seed other data if no elderly people exist
    if (context.ElderlyPeople.Any())
        return;

    // Get the users for creating relationships
    var allUsers = context.Users.ToList();

    // Create Elderly People
    var elderlyPeople = new List<ElderlyPerson>
    {
        new ElderlyPerson { Name = "Margaret Thompson", DateOfBirth = new DateTime(1935, 3, 15), PhoneNumber = "555-0201", EmergencyContactName = "John Thompson", EmergencyContactPhone = "555-0202", MedicalConditions = "Diabetes, Hypertension", Allergies = "Penicillin", Notes = "Current medications: Metformin, Lisinopril" },
        new ElderlyPerson { Name = "Robert Davis", DateOfBirth = new DateTime(1940, 7, 22), PhoneNumber = "555-0203", EmergencyContactName = "Mary Davis", EmergencyContactPhone = "555-0204", MedicalConditions = "Arthritis, Heart Disease", Allergies = "None", Notes = "Current medications: Aspirin, Ibuprofen" },
        new ElderlyPerson { Name = "Helen Brown", DateOfBirth = new DateTime(1938, 11, 8), PhoneNumber = "555-0205", EmergencyContactName = "James Brown", EmergencyContactPhone = "555-0206", MedicalConditions = "Dementia, Osteoporosis", Allergies = "Sulfa drugs", Notes = "Current medications: Donepezil, Calcium" }
    };

    context.ElderlyPeople.AddRange(elderlyPeople);
    context.SaveChanges();

    // Create Care Plans
    var carePlans = new List<CarePlan>
    {
        new CarePlan { ElderlyPersonId = elderlyPeople[0].Id, UserId = allUsers[0].Id, Title = "Diabetes Management Plan", Description = "Comprehensive care plan for diabetes management", Goals = "Maintain blood sugar levels, prevent complications", Interventions = "Regular monitoring, diet control, exercise", StartDate = DateTime.Today.AddDays(-30), EndDate = DateTime.Today.AddDays(60), Status = CarePlanStatus.Active, Priority = PriorityLevel.High },
        new CarePlan { ElderlyPersonId = elderlyPeople[1].Id, UserId = allUsers[1].Id, Title = "Heart Health Plan", Description = "Care plan for heart disease management", Goals = "Improve heart health, reduce risk factors", Interventions = "Medication compliance, lifestyle changes", StartDate = DateTime.Today.AddDays(-15), EndDate = DateTime.Today.AddDays(90), Status = CarePlanStatus.Active, Priority = PriorityLevel.High },
        new CarePlan { ElderlyPersonId = elderlyPeople[2].Id, UserId = allUsers[2].Id, Title = "Dementia Care Plan", Description = "Specialized care for dementia patients", Goals = "Maintain cognitive function, ensure safety", Interventions = "Memory exercises, safety measures", StartDate = DateTime.Today.AddDays(-45), EndDate = DateTime.Today.AddDays(120), Status = CarePlanStatus.Active, Priority = PriorityLevel.High }
    };

    context.CarePlans.AddRange(carePlans);
    context.SaveChanges();

    // Create Task Schedules
    var taskSchedules = new List<TaskSchedule>
    {
        new TaskSchedule { ElderlyPersonId = elderlyPeople[0].Id, UserId = allUsers[1].Id, CarePlanId = carePlans[0].Id, Title = "Blood Sugar Check", Description = "Morning blood glucose monitoring", ScheduledTime = DateTime.Today.AddHours(8), DurationMinutes = 15, Priority = PriorityLevel.High, Status = ElderlyCareApp.Models.TaskStatus.Scheduled, RecurrenceType = RecurrenceType.Daily },
        new TaskSchedule { ElderlyPersonId = elderlyPeople[0].Id, UserId = allUsers[2].Id, CarePlanId = carePlans[0].Id, Title = "Exercise Session", Description = "Light walking exercise", ScheduledTime = DateTime.Today.AddHours(10), DurationMinutes = 30, Priority = PriorityLevel.Medium, Status = ElderlyCareApp.Models.TaskStatus.Scheduled, RecurrenceType = RecurrenceType.Daily },
        new TaskSchedule { ElderlyPersonId = elderlyPeople[1].Id, UserId = allUsers[1].Id, CarePlanId = carePlans[1].Id, Title = "Blood Pressure Check", Description = "Monitor blood pressure", ScheduledTime = DateTime.Today.AddHours(9), DurationMinutes = 10, Priority = PriorityLevel.High, Status = ElderlyCareApp.Models.TaskStatus.Scheduled, RecurrenceType = RecurrenceType.Daily },
        new TaskSchedule { ElderlyPersonId = elderlyPeople[2].Id, UserId = allUsers[3].Id, CarePlanId = carePlans[2].Id, Title = "Memory Exercise", Description = "Cognitive stimulation activities", ScheduledTime = DateTime.Today.AddHours(14), DurationMinutes = 20, Priority = PriorityLevel.Medium, Status = ElderlyCareApp.Models.TaskStatus.Scheduled, RecurrenceType = RecurrenceType.Daily }
    };

    context.TaskSchedules.AddRange(taskSchedules);
    context.SaveChanges();

    // Create some sample logs
    var activityLogs = new List<ActivityLog>
    {
        new ActivityLog { ElderlyPersonId = elderlyPeople[0].Id, UserId = allUsers[2].Id, ActivityType = "Walking", StartTime = DateTime.Today.AddHours(-2), EndTime = DateTime.Today.AddHours(-1.5), Description = "Morning walk in the garden", Location = "Garden" },
        new ActivityLog { ElderlyPersonId = elderlyPeople[1].Id, UserId = allUsers[1].Id, ActivityType = "Exercise", StartTime = DateTime.Today.AddHours(-3), EndTime = DateTime.Today.AddHours(-2.5), Description = "Light stretching exercises", Location = "Activity Room" },
        new ActivityLog { ElderlyPersonId = elderlyPeople[2].Id, UserId = allUsers[3].Id, ActivityType = "Puzzle", StartTime = DateTime.Today.AddHours(-1), EndTime = DateTime.Today.AddHours(-0.5), Description = "Memory puzzle activity", Location = "Common Room" }
    };

    context.ActivityLogs.AddRange(activityLogs);
    context.SaveChanges();

    var medicationLogs = new List<MedicationLog>
    {
        new MedicationLog { ElderlyPersonId = elderlyPeople[0].Id, UserId = allUsers[1].Id, MedicationName = "Metformin", Dosage = "500mg", ScheduledTime = DateTime.Today.AddHours(8), Status = MedicationStatus.Taken, Notes = "Taken with breakfast" },
        new MedicationLog { ElderlyPersonId = elderlyPeople[1].Id, UserId = allUsers[1].Id, MedicationName = "Aspirin", Dosage = "81mg", ScheduledTime = DateTime.Today.AddHours(9), Status = MedicationStatus.Taken, Notes = "Daily low-dose aspirin" },
        new MedicationLog { ElderlyPersonId = elderlyPeople[2].Id, UserId = allUsers[1].Id, MedicationName = "Donepezil", Dosage = "5mg", ScheduledTime = DateTime.Today.AddHours(20), Status = MedicationStatus.Scheduled, Notes = "Evening dose for dementia" }
    };

    context.MedicationLogs.AddRange(medicationLogs);
    context.SaveChanges();

    var appointmentLogs = new List<AppointmentLog>
    {
        new AppointmentLog { ElderlyPersonId = elderlyPeople[0].Id, UserId = allUsers[0].Id, ScheduledDateTime = DateTime.Today.AddDays(3).AddHours(10), AppointmentType = AppointmentType.DoctorVisit, Title = "Diabetes Check", ProviderName = "Dr. Sarah Johnson", Location = "Medical Center", Status = AppointmentStatus.Scheduled, Notes = "Quarterly diabetes review" },
        new AppointmentLog { ElderlyPersonId = elderlyPeople[1].Id, UserId = allUsers[0].Id, ScheduledDateTime = DateTime.Today.AddDays(5).AddHours(14), AppointmentType = AppointmentType.SpecialistConsultation, Title = "Cardiology", ProviderName = "Dr. Robert Smith", Location = "Heart Clinic", Status = AppointmentStatus.Scheduled, Notes = "Heart health assessment" },
        new AppointmentLog { ElderlyPersonId = elderlyPeople[2].Id, UserId = allUsers[3].Id, ScheduledDateTime = DateTime.Today.AddDays(2).AddHours(11), AppointmentType = AppointmentType.TherapySession, Title = "Physical Therapy", ProviderName = "David Wilson", Location = "Rehabilitation Center", Status = AppointmentStatus.Scheduled, Notes = "Mobility assessment" }
    };

    context.AppointmentLogs.AddRange(appointmentLogs);
    context.SaveChanges();
}
