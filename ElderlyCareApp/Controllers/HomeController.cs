using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElderlyCareApp.Models;

namespace ElderlyCareApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = HttpContext.Session.GetInt32("CurrentUserId");
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }
            var selectedPatientId = HttpContext.Session.GetInt32("SelectedPatientId");
            if (selectedPatientId.HasValue)
            {
                return await PatientDashboard(selectedPatientId.Value);
            }
            return await PatientSelection();
        }

        public async Task<IActionResult> PatientSelection()
        {
            var currentUserId = HttpContext.Session.GetInt32("CurrentUserId");
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _context.ElderlyPeople.Where(p => p.IsActive);

            // Filter patients based on user role
            if (currentUserRole == "Family")
            {
                // Family users can see all patients (they own the app)
                // No filtering needed
            }
            else if (currentUserRole == "Caregiver")
            {
                // Caregivers can only see patients they are currently assigned to
                var currentDate = DateTime.Now.Date;
                query = query.Where(p => _context.CaregiverAssignments
                    .Where(ca => ca.CaregiverId == currentUserId.Value && 
                                ca.ElderlyPersonId == p.Id && 
                                ca.IsActive &&
                                ca.StartDate <= currentDate &&
                                (ca.EndDate == null || ca.EndDate >= currentDate))
                    .Any());
            }

            var patients = await query.ToListAsync();
            return View("PatientSelection", patients);
        }

        [HttpPost]
        public IActionResult SelectPatient(int patientId)
        {
            HttpContext.Session.SetInt32("SelectedPatientId", patientId);
            return RedirectToAction("Index");
        }

        public IActionResult ClearSelection()
        {
            HttpContext.Session.Remove("SelectedPatientId");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> PatientDashboard(int patientId)
        {
            // Debug: Check if any patients exist
            var allPatients = await _context.ElderlyPeople.ToListAsync();
            if (!allPatients.Any())
            {
                return NotFound("No patients found in database");
            }

            var patient = await _context.ElderlyPeople
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
            {
                return NotFound($"Patient with ID {patientId} not found. Available IDs: {string.Join(", ", allPatients.Select(p => p.Id))}");
            }

            try
            {
                var viewModel = new PatientDashboardViewModel
                {
                    Patient = patient,
                    RecentActivities = await _context.ActivityLogs
                        .Include(a => a.User)
                        .Where(a => a.ElderlyPersonId == patientId)
                        .OrderByDescending(a => a.StartTime)
                        .Take(5)
                        .ToListAsync() ?? new List<ActivityLog>(),
                    RecentMedications = await _context.MedicationLogs
                        .Include(m => m.User)
                        .Where(m => m.ElderlyPersonId == patientId)
                        .OrderByDescending(m => m.Timestamp)
                        .Take(5)
                        .ToListAsync() ?? new List<MedicationLog>(),
                    RecentMeals = await _context.MealLogs
                        .Include(m => m.User)
                        .Where(m => m.ElderlyPersonId == patientId)
                        .OrderByDescending(m => m.MealTime)
                        .Take(5)
                        .ToListAsync() ?? new List<MealLog>(),
                    RecentAppointments = await _context.AppointmentLogs
                        .Include(a => a.User)
                        .Where(a => a.ElderlyPersonId == patientId)
                        .OrderByDescending(a => a.ScheduledDateTime)
                        .Take(5)
                        .ToListAsync() ?? new List<AppointmentLog>(),
                    RecentNotes = await _context.CareNotes
                        .Include(n => n.User)
                        .Where(n => n.ElderlyPersonId == patientId)
                        .OrderByDescending(n => n.CreatedAt)
                        .Take(5)
                        .ToListAsync() ?? new List<CareNote>()
                };

                return View("PatientDashboard", viewModel);
            }
            catch (Exception ex)
            {
                // Log the exception (in a real app, you'd use a proper logging framework)
                return StatusCode(500, $"An error occurred while loading patient data: {ex.Message}");
            }
        }

        public async Task<IActionResult> SystemOverview()
        {
            var currentUserId = HttpContext.Session.GetInt32("CurrentUserId");
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            if (currentUserRole != "Caregiver")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var viewModel = new DashboardViewModel
            {
                TotalElderlyPeople = await _context.ElderlyPeople.CountAsync(),
                ActiveElderlyPeople = await _context.ElderlyPeople.Where(p => p.IsActive).CountAsync(),
                TotalActivities = await _context.ActivityLogs.CountAsync(),
                TotalMedications = await _context.MedicationLogs.CountAsync(),
                TotalMeals = await _context.MealLogs.CountAsync(),
                TotalAppointments = await _context.AppointmentLogs.CountAsync(),
                RecentActivities = await _context.ActivityLogs
                    .Include(a => a.ElderlyPerson)
                    .Include(a => a.User)
                    .OrderByDescending(a => a.StartTime)
                    .Take(10)
                    .ToListAsync(),
                RecentMedications = await _context.MedicationLogs
                    .Include(m => m.ElderlyPerson)
                    .Include(m => m.User)
                    .OrderByDescending(m => m.Timestamp)
                    .Take(10)
                    .ToListAsync(),
                RecentMeals = await _context.MealLogs
                    .Include(m => m.ElderlyPerson)
                    .Include(m => m.User)
                    .OrderByDescending(m => m.MealTime)
                    .Take(10)
                    .ToListAsync(),
                RecentAppointments = await _context.AppointmentLogs
                    .Include(a => a.ElderlyPerson)
                    .Include(a => a.User)
                    .OrderByDescending(a => a.ScheduledDateTime)
                    .Take(10)
                    .ToListAsync()
            };

            return View("SystemOverview", viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public class DashboardViewModel
        {
            public int TotalElderlyPeople { get; set; }
            public int ActiveElderlyPeople { get; set; }
            public int TotalActivities { get; set; }
            public int TotalMedications { get; set; }
            public int TotalMeals { get; set; }
            public int TotalAppointments { get; set; }
            public List<ActivityLog> RecentActivities { get; set; } = new List<ActivityLog>();
            public List<MedicationLog> RecentMedications { get; set; } = new List<MedicationLog>();
            public List<MealLog> RecentMeals { get; set; } = new List<MealLog>();
            public List<AppointmentLog> RecentAppointments { get; set; } = new List<AppointmentLog>();
        }

        public class PatientDashboardViewModel
        {
            public ElderlyPerson Patient { get; set; } = null!;
            public List<ActivityLog> RecentActivities { get; set; } = new List<ActivityLog>();
            public List<MedicationLog> RecentMedications { get; set; } = new List<MedicationLog>();
            public List<MealLog> RecentMeals { get; set; } = new List<MealLog>();
            public List<AppointmentLog> RecentAppointments { get; set; } = new List<AppointmentLog>();
            public List<CareNote> RecentNotes { get; set; } = new List<CareNote>();
        }
    }
}
