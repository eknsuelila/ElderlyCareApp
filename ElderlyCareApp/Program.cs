using Microsoft.EntityFrameworkCore;
using ElderlyCareApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Session
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

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    SeedData(context);
}

app.Run();

static void SeedData(ApplicationDbContext context)
{
    // Ensure demo users with passwords exist
    var demoEmails = new[] { "family@elderlycare.com", "caregiver@elderlycare.com" };
    var existingDemoUsers = context.Users.Where(u => demoEmails.Contains(u.Email)).ToList();
    
    if (!existingDemoUsers.Any())
    {
        var users = new List<User>
        {
            new User { Name = "Sarah Johnson", Email = "family@elderlycare.com", Password = "password123", PhoneNumber = "555-0101", Role = UserRole.Family },
            new User { Name = "Michael Chen", Email = "caregiver@elderlycare.com", Password = "password123", PhoneNumber = "555-0102", Role = UserRole.Caregiver }
        };
        context.Users.AddRange(users);
        context.SaveChanges();
    }
    
    // Only seed other data if no elderly people exist
    if (context.ElderlyPeople.Any())
        return;
    
    // Get the users for creating relationships
    var allUsers = context.Users.ToList();
    
    // Create elderly people with more realistic data
    var elderlyPeople = new List<ElderlyPerson>
    {
        new ElderlyPerson 
        { 
            Name = "Margaret Johnson", 
            DateOfBirth = new DateTime(1940, 5, 15),
            PhoneNumber = "555-0201",
            EmergencyContactName = "Sarah Johnson",
            EmergencyContactPhone = "555-0101",
            Allergies = "Penicillin, Shellfish",
            MedicalConditions = "Hypertension, Type 2 Diabetes, Mild Arthritis",
            Notes = "Likes morning walks, prefers low-sodium meals, enjoys crossword puzzles"
        },
        new ElderlyPerson 
        { 
            Name = "Robert Wilson", 
            DateOfBirth = new DateTime(1938, 8, 22),
            PhoneNumber = "555-0202",
            EmergencyContactName = "Michael Chen",
            EmergencyContactPhone = "555-0102",
            Allergies = "None",
            MedicalConditions = "Osteoarthritis, Mild COPD",
            Notes = "Enjoys reading mystery novels, needs assistance with stairs, prefers warm meals"
        },
        new ElderlyPerson 
        { 
            Name = "Eleanor Davis", 
            DateOfBirth = new DateTime(1945, 3, 10),
            PhoneNumber = "555-0203",
            EmergencyContactName = "Jennifer Davis",
            EmergencyContactPhone = "555-0103",
            Allergies = "Sulfa drugs",
            MedicalConditions = "Heart condition, Osteoporosis",
            Notes = "Very social, loves gardening, needs regular medication reminders"
        },
        new ElderlyPerson 
        { 
            Name = "James Thompson", 
            DateOfBirth = new DateTime(1942, 11, 28),
            PhoneNumber = "555-0204",
            EmergencyContactName = "Michael Chen",
            EmergencyContactPhone = "555-0102",
            Allergies = "Latex",
            MedicalConditions = "Parkinson's disease, Hypertension",
            Notes = "Needs assistance with daily activities, enjoys classical music, requires frequent monitoring"
        }
    };
    context.ElderlyPeople.AddRange(elderlyPeople);
    context.SaveChanges();
    
    // Get the elderly people for creating relationships
    var allElderlyPeople = context.ElderlyPeople.ToList();
    
    // Create comprehensive sample activities for the past week
    var activities = new List<ActivityLog>
    {
        // Margaret's activities
        new ActivityLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, ActivityType = ActivityType.Walk, Description = "Morning walk in the park", StartTime = DateTime.Now.AddDays(-1).AddHours(8), DurationMinutes = 30, Notes = "Good energy level, walked 0.5 miles" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, ActivityType = ActivityType.Reading, Description = "Read newspaper and crossword", StartTime = DateTime.Now.AddDays(-1).AddHours(10), DurationMinutes = 45, Notes = "Completed most of the crossword puzzle" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, ActivityType = ActivityType.Social, Description = "Phone call with daughter", StartTime = DateTime.Now.AddDays(-2).AddHours(14), DurationMinutes = 20, Notes = "Good conversation, seemed happy" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, ActivityType = ActivityType.Rest, Description = "Afternoon nap", StartTime = DateTime.Now.AddDays(-2).AddHours(13), DurationMinutes = 60, Notes = "Slept well, needed rest" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, ActivityType = ActivityType.Walk, Description = "Evening stroll", StartTime = DateTime.Now.AddDays(-3).AddHours(18), DurationMinutes = 20, Notes = "Short walk around the block" },
        
        // Robert's activities
        new ActivityLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Exercise, Description = "Gentle stretching exercises", StartTime = DateTime.Now.AddDays(-1).AddHours(9), DurationMinutes = 20, Notes = "Good flexibility today, no pain" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Reading, Description = "Read mystery novel", StartTime = DateTime.Now.AddDays(-1).AddHours(11), DurationMinutes = 60, Notes = "Enjoyed the book, read 3 chapters" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Social, Description = "Visit from neighbor", StartTime = DateTime.Now.AddDays(-2).AddHours(15), DurationMinutes = 30, Notes = "Good social interaction" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Rest, Description = "Rest period", StartTime = DateTime.Now.AddDays(-3).AddHours(14), DurationMinutes = 45, Notes = "Needed rest due to arthritis" },
        
        // Eleanor's activities
        new ActivityLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Social, Description = "Gardening in backyard", StartTime = DateTime.Now.AddDays(-1).AddHours(10), DurationMinutes = 40, Notes = "Planted some flowers, enjoyed the sunshine" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Reading, Description = "Read gardening magazine", StartTime = DateTime.Now.AddDays(-1).AddHours(16), DurationMinutes = 30, Notes = "Learned about new plants" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Social, Description = "Video call with grandchildren", StartTime = DateTime.Now.AddDays(-2).AddHours(17), DurationMinutes = 25, Notes = "Very happy to see the kids" },
        
        // James's activities
        new ActivityLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Exercise, Description = "Physical therapy exercises", StartTime = DateTime.Now.AddDays(-1).AddHours(11), DurationMinutes = 30, Notes = "Good progress with balance exercises" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Social, Description = "Music therapy session", StartTime = DateTime.Now.AddDays(-1).AddHours(15), DurationMinutes = 45, Notes = "Listened to classical music, seemed relaxed" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Rest, Description = "Rest period", StartTime = DateTime.Now.AddDays(-2).AddHours(13), DurationMinutes = 90, Notes = "Longer rest needed today" }
    };
    context.ActivityLogs.AddRange(activities);
    
    // Create comprehensive sample medications
    var medications = new List<MedicationLog>
    {
        // Margaret's medications
        new MedicationLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MedicationName = "Metformin", Dosage = "500mg", Taken = true, Timestamp = DateTime.Now.AddDays(-1).AddHours(8), Notes = "Taken with breakfast" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MedicationName = "Lisinopril", Dosage = "10mg", Taken = true, Timestamp = DateTime.Now.AddDays(-1).AddHours(8), Notes = "Taken with breakfast" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MedicationName = "Metformin", Dosage = "500mg", Taken = true, Timestamp = DateTime.Now.AddDays(-1).AddHours(20), Notes = "Taken with dinner" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MedicationName = "Lisinopril", Dosage = "10mg", Taken = true, Timestamp = DateTime.Now.AddDays(-2).AddHours(8), Notes = "Taken with breakfast" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MedicationName = "Metformin", Dosage = "500mg", Taken = false, Timestamp = DateTime.Now.AddDays(-2).AddHours(20), Notes = "Forgot to take evening dose" },
        
        // Robert's medications
        new MedicationLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MedicationName = "Ibuprofen", Dosage = "400mg", Taken = true, Timestamp = DateTime.Now.AddDays(-1).AddHours(9), Notes = "For arthritis pain" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MedicationName = "Ibuprofen", Dosage = "400mg", Taken = true, Timestamp = DateTime.Now.AddDays(-1).AddHours(21), Notes = "Evening dose for pain" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MedicationName = "Ibuprofen", Dosage = "400mg", Taken = true, Timestamp = DateTime.Now.AddDays(-2).AddHours(9), Notes = "Morning dose" },
        
        // Eleanor's medications
        new MedicationLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MedicationName = "Amlodipine", Dosage = "5mg", Taken = true, Timestamp = DateTime.Now.AddDays(-1).AddHours(8), Notes = "Blood pressure medication" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MedicationName = "Calcium", Dosage = "600mg", Taken = true, Timestamp = DateTime.Now.AddDays(-1).AddHours(8), Notes = "For osteoporosis" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MedicationName = "Amlodipine", Dosage = "5mg", Taken = true, Timestamp = DateTime.Now.AddDays(-2).AddHours(8), Notes = "Morning dose" },
        
        // James's medications
        new MedicationLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MedicationName = "Levodopa", Dosage = "100mg", Taken = true, Timestamp = DateTime.Now.AddDays(-1).AddHours(8), Notes = "Parkinson's medication" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MedicationName = "Levodopa", Dosage = "100mg", Taken = true, Timestamp = DateTime.Now.AddDays(-1).AddHours(12), Notes = "Midday dose" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MedicationName = "Levodopa", Dosage = "100mg", Taken = true, Timestamp = DateTime.Now.AddDays(-1).AddHours(16), Notes = "Afternoon dose" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MedicationName = "Lisinopril", Dosage = "5mg", Taken = true, Timestamp = DateTime.Now.AddDays(-1).AddHours(8), Notes = "Blood pressure medication" }
    };
    context.MedicationLogs.AddRange(medications);
    
    // Create comprehensive sample meals
    var meals = new List<MealLog>
    {
        // Margaret's meals
        new MealLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MealType = MealType.Breakfast, MealName = "Oatmeal with berries", Description = "Healthy breakfast with low sugar", MealTime = DateTime.Now.AddDays(-1).AddHours(7), Notes = "Good appetite, ate 90%" },
        new MealLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MealType = MealType.Lunch, MealName = "Grilled chicken salad", Description = "Light lunch with vegetables", MealTime = DateTime.Now.AddDays(-1).AddHours(12), Notes = "Ate most of it, enjoyed the salad" },
        new MealLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MealType = MealType.Dinner, MealName = "Baked salmon with rice", Description = "Heart-healthy dinner", MealTime = DateTime.Now.AddDays(-1).AddHours(18), Notes = "Good appetite, finished the meal" },
        new MealLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MealType = MealType.Breakfast, MealName = "Whole grain toast with avocado", Description = "Healthy breakfast", MealTime = DateTime.Now.AddDays(-2).AddHours(7), Notes = "Good appetite" },
        
        // Robert's meals
        new MealLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MealType = MealType.Breakfast, MealName = "Toast and coffee", Description = "Simple breakfast", MealTime = DateTime.Now.AddDays(-1).AddHours(8), Notes = "Enjoyed the coffee, ate 75%" },
        new MealLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MealType = MealType.Lunch, MealName = "Soup and sandwich", Description = "Warm soup with turkey sandwich", MealTime = DateTime.Now.AddDays(-1).AddHours(12), Notes = "Preferred the soup, ate 80%" },
        new MealLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MealType = MealType.Dinner, MealName = "Pasta with meat sauce", Description = "Warm dinner", MealTime = DateTime.Now.AddDays(-1).AddHours(18), Notes = "Good appetite, finished the meal" },
        
        // Eleanor's meals
        new MealLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MealType = MealType.Breakfast, MealName = "Yogurt with granola", Description = "Light breakfast", MealTime = DateTime.Now.AddDays(-1).AddHours(8), Notes = "Good appetite" },
        new MealLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MealType = MealType.Lunch, MealName = "Vegetable soup and crackers", Description = "Light lunch", MealTime = DateTime.Now.AddDays(-1).AddHours(12), Notes = "Enjoyed the soup" },
        new MealLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MealType = MealType.Dinner, MealName = "Grilled chicken with vegetables", Description = "Healthy dinner", MealTime = DateTime.Now.AddDays(-1).AddHours(18), Notes = "Good appetite, ate 85%" },
        
        // James's meals
        new MealLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MealType = MealType.Breakfast, MealName = "Scrambled eggs and toast", Description = "Protein-rich breakfast", MealTime = DateTime.Now.AddDays(-1).AddHours(8), Notes = "Needed assistance, ate 70%" },
        new MealLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MealType = MealType.Lunch, MealName = "Pureed soup and soft bread", Description = "Easy to eat meal", MealTime = DateTime.Now.AddDays(-1).AddHours(12), Notes = "Ate slowly but finished" },
        new MealLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MealType = MealType.Dinner, MealName = "Soft fish with mashed potatoes", Description = "Soft food dinner", MealTime = DateTime.Now.AddDays(-1).AddHours(18), Notes = "Needed assistance, ate 60%" }
    };
    context.MealLogs.AddRange(meals);
    
    // Create comprehensive sample appointments
    var appointments = new List<AppointmentLog>
    {
        // Margaret's appointments
        new AppointmentLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, AppointmentType = AppointmentType.DoctorVisit, Title = "Annual checkup", Description = "Routine health check and blood work", ProviderName = "Dr. Smith", Location = "Medical Center", ScheduledDateTime = DateTime.Now.AddDays(7), Status = AppointmentStatus.Scheduled, Notes = "Bring blood sugar readings from the past week" },
        new AppointmentLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, AppointmentType = AppointmentType.Dental, Title = "Dental cleaning", Description = "Regular dental cleaning and checkup", ProviderName = "Dr. Johnson", Location = "Dental Clinic", ScheduledDateTime = DateTime.Now.AddDays(14), Status = AppointmentStatus.Scheduled, Notes = "Last cleaning was 6 months ago" },
        
        // Robert's appointments
        new AppointmentLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, AppointmentType = AppointmentType.Therapy, Title = "Physical therapy", Description = "Arthritis management and mobility exercises", ProviderName = "Dr. Williams", Location = "Rehabilitation Center", ScheduledDateTime = DateTime.Now.AddDays(3), Status = AppointmentStatus.Scheduled, Notes = "Focus on knee and hip exercises" },
        new AppointmentLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, AppointmentType = AppointmentType.DoctorVisit, Title = "Rheumatologist visit", Description = "Arthritis treatment review", ProviderName = "Dr. Brown", Location = "Specialty Clinic", ScheduledDateTime = DateTime.Now.AddDays(10), Status = AppointmentStatus.Scheduled, Notes = "Discuss new treatment options" },
        
        // Eleanor's appointments
        new AppointmentLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, AppointmentType = AppointmentType.DoctorVisit, Title = "Cardiologist follow-up", Description = "Heart condition monitoring", ProviderName = "Dr. Davis", Location = "Cardiology Center", ScheduledDateTime = DateTime.Now.AddDays(5), Status = AppointmentStatus.Scheduled, Notes = "Bring recent blood pressure readings" },
        new AppointmentLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, AppointmentType = AppointmentType.Dental, Title = "Dental checkup", Description = "Regular dental examination", ProviderName = "Dr. Wilson", Location = "Dental Office", ScheduledDateTime = DateTime.Now.AddDays(21), Status = AppointmentStatus.Scheduled, Notes = "Check for any issues" },
        
        // James's appointments
        new AppointmentLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, AppointmentType = AppointmentType.Specialist, Title = "Neurologist visit", Description = "Parkinson's disease management", ProviderName = "Dr. Anderson", Location = "Neurology Clinic", ScheduledDateTime = DateTime.Now.AddDays(2), Status = AppointmentStatus.Scheduled, Notes = "Review medication effectiveness" },
        new AppointmentLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, AppointmentType = AppointmentType.Therapy, Title = "Occupational therapy", Description = "Daily living skills assessment", ProviderName = "Dr. Martinez", Location = "Therapy Center", ScheduledDateTime = DateTime.Now.AddDays(8), Status = AppointmentStatus.Scheduled, Notes = "Focus on dressing and eating skills" }
    };
    context.AppointmentLogs.AddRange(appointments);
    
    // Create comprehensive sample care notes
    var careNotes = new List<CareNote>
    {
        // Margaret's notes
        new CareNote { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, NoteType = NoteType.Health, Title = "Blood sugar monitoring", Content = "Blood sugar levels have been stable this week, ranging from 120-140 mg/dL. Continue monitoring before meals and maintain current diet.", CreatedAt = DateTime.Now.AddDays(-2) },
        new CareNote { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, NoteType = NoteType.General, Title = "Good mood today", Content = "Margaret was in excellent spirits today. She enjoyed her morning walk and was very chatty during lunch. Completed most of her crossword puzzle.", CreatedAt = DateTime.Now.AddDays(-1) },
        new CareNote { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, NoteType = NoteType.Behavior, Title = "Increased social interaction", Content = "Margaret has been more social lately, calling her daughter daily and showing interest in community activities.", CreatedAt = DateTime.Now.AddDays(-3) },
        
        // Robert's notes
        new CareNote { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, NoteType = NoteType.Health, Title = "Arthritis flare-up", Content = "Robert experienced some knee pain today, especially in the morning. Applied heat therapy and gave ibuprofen as prescribed. Pain improved by afternoon.", CreatedAt = DateTime.Now.AddDays(-1) },
        new CareNote { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, NoteType = NoteType.Mood, Title = "Good spirits", Content = "Robert was in good spirits today, enjoyed reading his mystery novel and had a pleasant visit with his neighbor.", CreatedAt = DateTime.Now.AddDays(-2) },
        new CareNote { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, NoteType = NoteType.Important, Title = "Mobility assessment", Content = "Robert's mobility has been stable. He can walk short distances with his cane and manages stairs with assistance.", CreatedAt = DateTime.Now.AddDays(-4) },
        
        // Eleanor's notes
        new CareNote { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, NoteType = NoteType.Health, Title = "Blood pressure stable", Content = "Eleanor's blood pressure has been well-controlled this week, averaging 130/80. Continue current medication regimen.", CreatedAt = DateTime.Now.AddDays(-1) },
        new CareNote { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, NoteType = NoteType.Behavior, Title = "Very social and active", Content = "Eleanor has been very social and active this week. She enjoys gardening and had a wonderful video call with her grandchildren.", CreatedAt = DateTime.Now.AddDays(-2) },
        new CareNote { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, NoteType = NoteType.Important, Title = "Fall prevention", Content = "Eleanor is at risk for falls due to osteoporosis. Ensure clear pathways and good lighting in her living areas.", CreatedAt = DateTime.Now.AddDays(-5) },
        
        // James's notes
        new CareNote { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, NoteType = NoteType.Health, Title = "Parkinson's symptoms", Content = "James's tremors have been more noticeable today, especially in the morning. Medication timing seems to be working well.", CreatedAt = DateTime.Now.AddDays(-1) },
        new CareNote { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, NoteType = NoteType.Behavior, Title = "Good response to music therapy", Content = "James responded very well to today's music therapy session. He seemed relaxed and engaged while listening to classical music.", CreatedAt = DateTime.Now.AddDays(-1) },
        new CareNote { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, NoteType = NoteType.Important, Title = "Medication timing critical", Content = "James's Parkinson's medication must be given at exact times (8am, 12pm, 4pm, 8pm) for optimal symptom control.", CreatedAt = DateTime.Now.AddDays(-3) }
    };
    context.CareNotes.AddRange(careNotes);
    
    // Create caregiver assignments (Family owns the app, hires caregivers)
    var caregiverAssignments = new List<CaregiverAssignment>
    {
        // Michael Chen is assigned to Robert Wilson (ongoing)
        new CaregiverAssignment 
        { 
            CaregiverId = allUsers[1].Id, 
            ElderlyPersonId = allElderlyPeople[1].Id, 
            StartDate = DateTime.Now.AddDays(-30), 
            EndDate = null, // Ongoing assignment
            Notes = "Primary caregiver for Robert Wilson",
            IsActive = true 
        },
        
        // Michael Chen is assigned to Eleanor Davis (ongoing)
        new CaregiverAssignment 
        { 
            CaregiverId = allUsers[1].Id, 
            ElderlyPersonId = allElderlyPeople[2].Id, 
            StartDate = DateTime.Now.AddDays(-15), 
            EndDate = null, // Ongoing assignment
            Notes = "Primary caregiver for Eleanor Davis",
            IsActive = true 
        },
        
        // Michael Chen is assigned to James Thompson (ongoing)
        new CaregiverAssignment 
        { 
            CaregiverId = allUsers[1].Id, 
            ElderlyPersonId = allElderlyPeople[3].Id, 
            StartDate = DateTime.Now.AddDays(-7), 
            EndDate = null, // Ongoing assignment
            Notes = "Primary caregiver for James Thompson",
            IsActive = true 
        },
        
        // Note: Margaret Johnson is cared for by Sarah Johnson (Family member)
        // No formal assignment needed as family owns the app
    };
    context.CaregiverAssignments.AddRange(caregiverAssignments);
    
    context.SaveChanges();
}
