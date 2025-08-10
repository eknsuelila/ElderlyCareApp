using Microsoft.EntityFrameworkCore;
using ElderlyCareApp.Models;
using ElderlyCareApp.Services;

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

// Add AI Health Service
builder.Services.AddHttpClient<AIHealthService>(client =>
{
    client.Timeout = TimeSpan.FromMinutes(2); // 2 minute timeout
    client.DefaultRequestHeaders.Add("User-Agent", "ElderlyCareApp/1.0");
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
    
    // Add recent data for today and yesterday
    var today = DateTime.Now.Date;
    var yesterday = DateTime.Now.AddDays(-1).Date;
    
    // Today's activities
    var todayActivities = new List<ActivityLog>
    {
        // Margaret - Today
        new ActivityLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, ActivityType = ActivityType.Walk, Description = "Morning walk in garden", StartTime = today.AddHours(8), EndTime = today.AddHours(9), Notes = "Good pace, enjoyed the fresh air" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, ActivityType = ActivityType.Reading, Description = "Crossword puzzle", StartTime = today.AddHours(10), EndTime = today.AddHours(11), Notes = "Completed 80% of puzzle" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, ActivityType = ActivityType.Social, Description = "Video call with daughter", StartTime = today.AddHours(14), EndTime = today.AddHours(14).AddMinutes(30), Notes = "Very happy to see family" },
        
        // Robert - Today
        new ActivityLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Exercise, Description = "Gentle stretching", StartTime = today.AddHours(9), EndTime = today.AddHours(9).AddMinutes(30), Notes = "Helped with arthritis pain" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Reading, Description = "Mystery novel", StartTime = today.AddHours(11), EndTime = today.AddHours(12), Notes = "Read 2 chapters" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Social, Description = "Neighbor visit", StartTime = today.AddHours(15), EndTime = today.AddHours(16), Notes = "Good conversation" },
        
        // Eleanor - Today
        new ActivityLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Walk, Description = "Garden walk", StartTime = today.AddHours(8), EndTime = today.AddHours(8).AddMinutes(45), Notes = "Enjoyed the flowers" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Social, Description = "Grandchildren video call", StartTime = today.AddHours(13), EndTime = today.AddHours(13).AddMinutes(45), Notes = "Very excited to see kids" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Exercise, Description = "Light yoga", StartTime = today.AddHours(16), EndTime = today.AddHours(16).AddMinutes(30), Notes = "Good for flexibility" },
        
        // James - Today
        new ActivityLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Exercise, Description = "Physical therapy exercises", StartTime = today.AddHours(10), EndTime = today.AddHours(10).AddMinutes(45), Notes = "Focused on balance" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Social, Description = "Music therapy session", StartTime = today.AddHours(14), EndTime = today.AddHours(14).AddMinutes(30), Notes = "Very responsive to classical music" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Rest, Description = "Afternoon rest", StartTime = today.AddHours(15), EndTime = today.AddHours(16), Notes = "Needed after therapy" }
    };
    context.ActivityLogs.AddRange(todayActivities);
    
    // Yesterday's activities
    var yesterdayActivities = new List<ActivityLog>
    {
        // Margaret - Yesterday
        new ActivityLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, ActivityType = ActivityType.Walk, Description = "Short walk around house", StartTime = yesterday.AddHours(9), EndTime = yesterday.AddHours(9).AddMinutes(30), Notes = "Felt tired, shorter walk" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, ActivityType = ActivityType.Reading, Description = "Magazine reading", StartTime = yesterday.AddHours(11), EndTime = yesterday.AddHours(12), Notes = "Read health articles" },
        
        // Robert - Yesterday
        new ActivityLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Exercise, Description = "Chair exercises", StartTime = yesterday.AddHours(10), EndTime = yesterday.AddHours(10).AddMinutes(20), Notes = "Knee pain, modified exercises" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Reading, Description = "Newspaper reading", StartTime = yesterday.AddHours(14), EndTime = yesterday.AddHours(15), Notes = "Read local news" },
        
        // Eleanor - Yesterday
        new ActivityLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Walk, Description = "Indoor walking", StartTime = yesterday.AddHours(8), EndTime = yesterday.AddHours(8).AddMinutes(40), Notes = "Rainy day, walked indoors" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Social, Description = "Phone call with friend", StartTime = yesterday.AddHours(13), EndTime = yesterday.AddHours(13).AddMinutes(25), Notes = "Good conversation" },
        
        // James - Yesterday
        new ActivityLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Exercise, Description = "Balance exercises", StartTime = yesterday.AddHours(9), EndTime = yesterday.AddHours(9).AddMinutes(30), Notes = "Stable during exercises" },
        new ActivityLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, ActivityType = ActivityType.Rest, Description = "Morning rest", StartTime = yesterday.AddHours(11), EndTime = yesterday.AddHours(12), Notes = "Needed rest after exercises" }
    };
    context.ActivityLogs.AddRange(yesterdayActivities);
    
    // Today's medications
    var todayMedications = new List<MedicationLog>
    {
        // Margaret - Today
        new MedicationLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MedicationName = "Metformin", Dosage = "500mg", Taken = true, Timestamp = today.AddHours(8), Notes = "Taken with breakfast" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MedicationName = "Lisinopril", Dosage = "10mg", Taken = true, Timestamp = today.AddHours(8), Notes = "Taken with breakfast" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MedicationName = "Metformin", Dosage = "500mg", Taken = true, Timestamp = today.AddHours(20), Notes = "Taken with dinner" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MedicationName = "Lisinopril", Dosage = "10mg", Taken = false, Timestamp = today.AddHours(20), Notes = "Forgot evening dose" },
        
        // Robert - Today
        new MedicationLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MedicationName = "Ibuprofen", Dosage = "400mg", Taken = true, Timestamp = today.AddHours(8), Notes = "For arthritis pain" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MedicationName = "Ibuprofen", Dosage = "400mg", Taken = true, Timestamp = today.AddHours(20), Notes = "Evening dose" },
        
        // Eleanor - Today
        new MedicationLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MedicationName = "Amlodipine", Dosage = "5mg", Taken = true, Timestamp = today.AddHours(8), Notes = "Blood pressure medication" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MedicationName = "Calcium", Dosage = "600mg", Taken = true, Timestamp = today.AddHours(8), Notes = "For osteoporosis" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MedicationName = "Amlodipine", Dosage = "5mg", Taken = true, Timestamp = today.AddHours(20), Notes = "Evening dose" },
        
        // James - Today
        new MedicationLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MedicationName = "Levodopa", Dosage = "100mg", Taken = true, Timestamp = today.AddHours(8), Notes = "Parkinson's medication" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MedicationName = "Levodopa", Dosage = "100mg", Taken = true, Timestamp = today.AddHours(12), Notes = "Midday dose" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MedicationName = "Levodopa", Dosage = "100mg", Taken = true, Timestamp = today.AddHours(16), Notes = "Afternoon dose" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MedicationName = "Levodopa", Dosage = "100mg", Taken = false, Timestamp = today.AddHours(20), Notes = "Missed evening dose" }
    };
    context.MedicationLogs.AddRange(todayMedications);
    
    // Yesterday's medications
    var yesterdayMedications = new List<MedicationLog>
    {
        // Margaret - Yesterday
        new MedicationLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MedicationName = "Metformin", Dosage = "500mg", Taken = true, Timestamp = yesterday.AddHours(8), Notes = "Taken with breakfast" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MedicationName = "Lisinopril", Dosage = "10mg", Taken = true, Timestamp = yesterday.AddHours(8), Notes = "Taken with breakfast" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MedicationName = "Metformin", Dosage = "500mg", Taken = true, Timestamp = yesterday.AddHours(20), Notes = "Taken with dinner" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MedicationName = "Lisinopril", Dosage = "10mg", Taken = true, Timestamp = yesterday.AddHours(20), Notes = "Taken with dinner" },
        
        // Robert - Yesterday
        new MedicationLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MedicationName = "Ibuprofen", Dosage = "400mg", Taken = true, Timestamp = yesterday.AddHours(8), Notes = "For arthritis pain" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MedicationName = "Ibuprofen", Dosage = "400mg", Taken = true, Timestamp = yesterday.AddHours(20), Notes = "Evening dose" },
        
        // Eleanor - Yesterday
        new MedicationLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MedicationName = "Amlodipine", Dosage = "5mg", Taken = true, Timestamp = yesterday.AddHours(8), Notes = "Blood pressure medication" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MedicationName = "Calcium", Dosage = "600mg", Taken = true, Timestamp = yesterday.AddHours(8), Notes = "For osteoporosis" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MedicationName = "Amlodipine", Dosage = "5mg", Taken = true, Timestamp = yesterday.AddHours(20), Notes = "Evening dose" },
        
        // James - Yesterday
        new MedicationLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MedicationName = "Levodopa", Dosage = "100mg", Taken = true, Timestamp = yesterday.AddHours(8), Notes = "Parkinson's medication" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MedicationName = "Levodopa", Dosage = "100mg", Taken = true, Timestamp = yesterday.AddHours(12), Notes = "Midday dose" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MedicationName = "Levodopa", Dosage = "100mg", Taken = true, Timestamp = yesterday.AddHours(16), Notes = "Afternoon dose" },
        new MedicationLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MedicationName = "Levodopa", Dosage = "100mg", Taken = true, Timestamp = yesterday.AddHours(20), Notes = "Evening dose" }
    };
    context.MedicationLogs.AddRange(yesterdayMedications);
    
    // Today's meals
    var todayMeals = new List<MealLog>
    {
        // Margaret - Today
        new MealLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MealType = MealType.Breakfast, MealName = "Oatmeal with berries", Description = "Healthy breakfast", MealTime = today.AddHours(8), Notes = "Good appetite, ate 90%" },
        new MealLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MealType = MealType.Lunch, MealName = "Grilled chicken salad", Description = "Light lunch", MealTime = today.AddHours(12), Notes = "Enjoyed the salad, ate 85%" },
        new MealLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MealType = MealType.Dinner, MealName = "Salmon with vegetables", Description = "Healthy dinner", MealTime = today.AddHours(18), Notes = "Good appetite, finished meal" },
        
        // Robert - Today
        new MealLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MealType = MealType.Breakfast, MealName = "Toast and coffee", Description = "Simple breakfast", MealTime = today.AddHours(8), Notes = "Enjoyed the coffee, ate 75%" },
        new MealLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MealType = MealType.Lunch, MealName = "Soup and sandwich", Description = "Warm soup with turkey sandwich", MealTime = today.AddHours(12), Notes = "Preferred the soup, ate 80%" },
        new MealLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MealType = MealType.Dinner, MealName = "Pasta with meat sauce", Description = "Warm dinner", MealTime = today.AddHours(18), Notes = "Good appetite, finished the meal" },
        
        // Eleanor - Today
        new MealLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MealType = MealType.Breakfast, MealName = "Yogurt with granola", Description = "Light breakfast", MealTime = today.AddHours(8), Notes = "Good appetite" },
        new MealLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MealType = MealType.Lunch, MealName = "Vegetable soup and crackers", Description = "Light lunch", MealTime = today.AddHours(12), Notes = "Enjoyed the soup" },
        new MealLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MealType = MealType.Dinner, MealName = "Grilled chicken with vegetables", Description = "Healthy dinner", MealTime = today.AddHours(18), Notes = "Good appetite, ate 85%" },
        
        // James - Today
        new MealLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MealType = MealType.Breakfast, MealName = "Scrambled eggs and toast", Description = "Protein-rich breakfast", MealTime = today.AddHours(8), Notes = "Needed assistance, ate 70%" },
        new MealLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MealType = MealType.Lunch, MealName = "Pureed soup and soft bread", Description = "Easy to eat meal", MealTime = today.AddHours(12), Notes = "Ate slowly but finished" },
        new MealLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MealType = MealType.Dinner, MealName = "Soft fish with mashed potatoes", Description = "Soft food dinner", MealTime = today.AddHours(18), Notes = "Needed assistance, ate 60%" }
    };
    context.MealLogs.AddRange(todayMeals);
    
    // Yesterday's meals
    var yesterdayMeals = new List<MealLog>
    {
        // Margaret - Yesterday
        new MealLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MealType = MealType.Breakfast, MealName = "Cereal with milk", Description = "Quick breakfast", MealTime = yesterday.AddHours(8), Notes = "Ate 80%" },
        new MealLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MealType = MealType.Lunch, MealName = "Tuna sandwich", Description = "Light lunch", MealTime = yesterday.AddHours(12), Notes = "Good appetite" },
        new MealLog { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, MealType = MealType.Dinner, MealName = "Baked chicken with rice", Description = "Warm dinner", MealTime = yesterday.AddHours(18), Notes = "Enjoyed the meal" },
        
        // Robert - Yesterday
        new MealLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MealType = MealType.Breakfast, MealName = "Pancakes and coffee", Description = "Comfortable breakfast", MealTime = yesterday.AddHours(8), Notes = "Enjoyed the pancakes" },
        new MealLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MealType = MealType.Lunch, MealName = "Ham and cheese sandwich", Description = "Simple lunch", MealTime = yesterday.AddHours(12), Notes = "Good appetite" },
        new MealLog { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, MealType = MealType.Dinner, MealName = "Beef stew with bread", Description = "Warm comfort food", MealTime = yesterday.AddHours(18), Notes = "Liked the stew" },
        
        // Eleanor - Yesterday
        new MealLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MealType = MealType.Breakfast, MealName = "Fruit and yogurt", Description = "Light breakfast", MealTime = yesterday.AddHours(8), Notes = "Good appetite" },
        new MealLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MealType = MealType.Lunch, MealName = "Chicken noodle soup", Description = "Warm soup", MealTime = yesterday.AddHours(12), Notes = "Enjoyed the soup" },
        new MealLog { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, MealType = MealType.Dinner, MealName = "Baked fish with vegetables", Description = "Healthy dinner", MealTime = yesterday.AddHours(18), Notes = "Good appetite" },
        
        // James - Yesterday
        new MealLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MealType = MealType.Breakfast, MealName = "Oatmeal with honey", Description = "Soft breakfast", MealTime = yesterday.AddHours(8), Notes = "Needed assistance, ate 65%" },
        new MealLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MealType = MealType.Lunch, MealName = "Cream of mushroom soup", Description = "Easy to eat", MealTime = yesterday.AddHours(12), Notes = "Ate slowly but finished" },
        new MealLog { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, MealType = MealType.Dinner, MealName = "Soft meatloaf with gravy", Description = "Soft food dinner", MealTime = yesterday.AddHours(18), Notes = "Needed assistance, ate 55%" }
    };
    context.MealLogs.AddRange(yesterdayMeals);
    
    // Today's care notes
    var todayCareNotes = new List<CareNote>
    {
        // Margaret - Today
        new CareNote { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, NoteType = NoteType.Health, Title = "Blood sugar check", Content = "Blood sugar was 135 mg/dL this morning, which is within normal range. Continue monitoring.", CreatedAt = today.AddHours(8) },
        new CareNote { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, NoteType = NoteType.Mood, Title = "Excellent mood today", Content = "Margaret was very cheerful today, especially after the video call with her daughter. She completed her crossword puzzle and enjoyed her garden walk.", CreatedAt = today.AddHours(16) },
        
        // Robert - Today
        new CareNote { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, NoteType = NoteType.Health, Title = "Arthritis management", Content = "Robert's knee pain was manageable today. The morning stretching exercises helped. Applied heat therapy as needed.", CreatedAt = today.AddHours(10) },
        new CareNote { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, NoteType = NoteType.Behavior, Title = "Good social interaction", Content = "Robert had a pleasant visit with his neighbor today. They discussed books and shared coffee. This social interaction is very beneficial for his mood.", CreatedAt = today.AddHours(16) },
        
        // Eleanor - Today
        new CareNote { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, NoteType = NoteType.Health, Title = "Blood pressure monitoring", Content = "Eleanor's blood pressure was 128/78 today, which is excellent. Continue current medication regimen.", CreatedAt = today.AddHours(9) },
        new CareNote { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, NoteType = NoteType.Behavior, Title = "Very active and social", Content = "Eleanor was very active today. She enjoyed her garden walk, had a wonderful video call with her grandchildren, and did light yoga. Her mood was excellent throughout the day.", CreatedAt = today.AddHours(17) },
        
        // James - Today
        new CareNote { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, NoteType = NoteType.Health, Title = "Parkinson's symptoms", Content = "James's tremors were moderate today, especially in the morning. The medication timing is working well, with symptoms improving after each dose.", CreatedAt = today.AddHours(11) },
        new CareNote { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, NoteType = NoteType.Behavior, Title = "Excellent response to music therapy", Content = "James had an excellent music therapy session today. He was very responsive to classical music, showing reduced tremors and improved mood during the session.", CreatedAt = today.AddHours(15) }
    };
    context.CareNotes.AddRange(todayCareNotes);
    
    // Yesterday's care notes
    var yesterdayCareNotes = new List<CareNote>
    {
        // Margaret - Yesterday
        new CareNote { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, NoteType = NoteType.Health, Title = "Blood sugar monitoring", Content = "Blood sugar was 142 mg/dL yesterday morning, slightly elevated but within acceptable range. Continue monitoring.", CreatedAt = yesterday.AddHours(8) },
        new CareNote { ElderlyPersonId = allElderlyPeople[0].Id, UserId = allUsers[0].Id, NoteType = NoteType.Mood, Title = "Tired but content", Content = "Margaret felt a bit tired yesterday but was content. She enjoyed reading her magazine and had a good appetite for meals.", CreatedAt = yesterday.AddHours(16) },
        
        // Robert - Yesterday
        new CareNote { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, NoteType = NoteType.Health, Title = "Arthritis flare-up", Content = "Robert experienced some knee pain yesterday, especially in the morning. Applied heat therapy and gave ibuprofen as prescribed. Pain improved by afternoon.", CreatedAt = yesterday.AddHours(9) },
        new CareNote { ElderlyPersonId = allElderlyPeople[1].Id, UserId = allUsers[1].Id, NoteType = NoteType.Behavior, Title = "Good spirits despite pain", Content = "Despite the arthritis pain, Robert maintained good spirits yesterday. He enjoyed reading the newspaper and had a pleasant phone conversation.", CreatedAt = yesterday.AddHours(15) },
        
        // Eleanor - Yesterday
        new CareNote { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, NoteType = NoteType.Health, Title = "Blood pressure stable", Content = "Eleanor's blood pressure was 132/80 yesterday, which is well-controlled. Continue current medication regimen.", CreatedAt = yesterday.AddHours(9) },
        new CareNote { ElderlyPersonId = allElderlyPeople[2].Id, UserId = allUsers[1].Id, NoteType = NoteType.Behavior, Title = "Good social interaction", Content = "Eleanor had a pleasant phone call with her friend yesterday. She also enjoyed indoor walking despite the rainy weather.", CreatedAt = yesterday.AddHours(14) },
        
        // James - Yesterday
        new CareNote { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, NoteType = NoteType.Health, Title = "Parkinson's management", Content = "James's Parkinson's symptoms were well-controlled yesterday. All medications were taken on time, and he had good symptom control throughout the day.", CreatedAt = yesterday.AddHours(10) },
        new CareNote { ElderlyPersonId = allElderlyPeople[3].Id, UserId = allUsers[1].Id, NoteType = NoteType.Behavior, Title = "Stable and responsive", Content = "James was stable and responsive yesterday. He completed his balance exercises well and rested appropriately. His medication compliance was excellent.", CreatedAt = yesterday.AddHours(16) }
    };
    context.CareNotes.AddRange(yesterdayCareNotes);
    
    context.SaveChanges();
}
