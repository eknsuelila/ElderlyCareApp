using System.Diagnostics;
using ElderlyCareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElderlyCareApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Check if user is logged in
            var currentUserId = HttpContext.Session.GetInt32("CurrentUserId");
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if a patient is selected
            var selectedPatientId = HttpContext.Session.GetInt32("SelectedPatientId");
            
            if (selectedPatientId.HasValue)
            {
                // Show personalized dashboard for selected patient
                return await PatientDashboard(selectedPatientId.Value);
            }
            
            // Show patient selection page
            return await PatientSelection();
        }

        public async Task<IActionResult> PatientSelection()
        {
            // Check if user is logged in
            var currentUserId = HttpContext.Session.GetInt32("CurrentUserId");
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Get patients based on user role
            IQueryable<ElderlyPerson> patientsQuery = _context.ElderlyPeople.Where(p => p.IsActive);
            
            // Family members can only see their own family members
            if (currentUserRole == "Family")
            {
                // For demo purposes, family members can see all patients
                // In a real system, you'd filter by family relationships
                patientsQuery = patientsQuery.Where(p => p.IsActive);
            }
            // Caregivers can see all patients they're assigned to
            else if (currentUserRole == "Caregiver")
            {
                // For demo purposes, caregivers can see all patients
                // In a real system, you'd filter by caregiver assignments
                patientsQuery = patientsQuery.Where(p => p.IsActive);
            }

            var patients = await patientsQuery.OrderBy(p => p.Name).ToListAsync();
            return View("PatientSelection", patients);
        }

        [HttpPost]
        public IActionResult SelectPatient(int patientId)
        {
            // Check if user is logged in
            var currentUserId = HttpContext.Session.GetInt32("CurrentUserId");
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            HttpContext.Session.SetInt32("SelectedPatientId", patientId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ClearSelection()
        {
            HttpContext.Session.Remove("SelectedPatientId");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PatientDashboard(int patientId)
        {
            // Check if user is logged in
            var currentUserId = HttpContext.Session.GetInt32("CurrentUserId");
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = await _context.ElderlyPeople
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
            {
                HttpContext.Session.Remove("SelectedPatientId");
                return RedirectToAction(nameof(Index));
            }

            var dashboardViewModel = new PatientDashboardViewModel
            {
                Patient = patient,
                
                // Patient-specific statistics
                TotalCarePlans = await _context.CarePlans
                    .Where(c => c.ElderlyPersonId == patientId)
                    .CountAsync(),
                
                ActiveCarePlans = await _context.CarePlans
                    .Where(c => c.ElderlyPersonId == patientId && c.Status == CarePlanStatus.Active)
                    .CountAsync(),
                
                PendingTasks = await _context.TaskSchedules
                    .Where(t => t.ElderlyPersonId == patientId && 
                               t.Status == Models.TaskStatus.Scheduled && 
                               t.ScheduledTime >= DateTime.Today)
                    .CountAsync(),
                
                TodayTasks = await _context.TaskSchedules
                    .Include(t => t.User)
                    .Where(t => t.ElderlyPersonId == patientId && 
                               t.ScheduledTime.Date == DateTime.Today)
                    .OrderBy(t => t.ScheduledTime)
                    .ToListAsync(),
                
                RecentActivityLogs = await _context.ActivityLogs
                    .Include(a => a.User)
                    .Where(a => a.ElderlyPersonId == patientId)
                    .OrderByDescending(a => a.StartTime)
                    .Take(5)
                    .ToListAsync(),
                
                RecentMedicationLogs = await _context.MedicationLogs
                    .Include(m => m.User)
                    .Where(m => m.ElderlyPersonId == patientId)
                    .OrderByDescending(m => m.ScheduledTime)
                    .Take(5)
                    .ToListAsync(),
                
                UpcomingAppointments = await _context.AppointmentLogs
                    .Include(a => a.User)
                    .Where(a => a.ElderlyPersonId == patientId && 
                               a.ScheduledDateTime >= DateTime.Now && 
                               a.Status == AppointmentStatus.Scheduled)
                    .OrderBy(a => a.ScheduledDateTime)
                    .Take(5)
                    .ToListAsync(),
                
                RecentVitalSigns = await _context.VitalSignsLogs
                    .Where(v => v.ElderlyPersonId == patientId)
                    .OrderByDescending(v => v.MeasurementTime)
                    .Take(3)
                    .ToListAsync(),
                
                RecentSymptoms = await _context.SymptomLogs
                    .Where(s => s.ElderlyPersonId == patientId)
                    .OrderByDescending(s => s.LogTime)
                    .Take(3)
                    .ToListAsync()
            };

            return View("PatientDashboard", dashboardViewModel);
        }

        public async Task<IActionResult> SystemOverview()
        {
            // Check if user is logged in
            var currentUserId = HttpContext.Session.GetInt32("CurrentUserId");
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Only caregivers can access system overview
            if (currentUserRole != "Caregiver")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var dashboardViewModel = new DashboardViewModel
            {
                TotalElderlyPeople = await _context.ElderlyPeople.CountAsync(),
                TotalUsers = await _context.Users.CountAsync(),
                TotalCarePlans = await _context.CarePlans.CountAsync(),
                PendingTasks = await _context.TaskSchedules
                    .Where(t => t.Status == Models.TaskStatus.Scheduled && t.ScheduledTime >= DateTime.Today)
                    .CountAsync(),
                
                RecentActivityLogs = await _context.ActivityLogs
                    .Include(a => a.ElderlyPerson)
                    .OrderByDescending(a => a.StartTime)
                    .Take(5)
                    .ToListAsync(),
                
                RecentMedicationLogs = await _context.MedicationLogs
                    .Include(m => m.ElderlyPerson)
                    .OrderByDescending(m => m.ScheduledTime)
                    .Take(5)
                    .ToListAsync(),
                
                UpcomingAppointments = await _context.AppointmentLogs
                    .Include(a => a.ElderlyPerson)
                    .Where(a => a.ScheduledDateTime >= DateTime.Now && a.Status == AppointmentStatus.Scheduled)
                    .OrderBy(a => a.ScheduledDateTime)
                    .Take(5)
                    .ToListAsync(),
                
                TodayTasks = await _context.TaskSchedules
                    .Include(t => t.ElderlyPerson)
                    .Include(t => t.User)
                    .Where(t => t.ScheduledTime.Date == DateTime.Today)
                    .OrderBy(t => t.ScheduledTime)
                    .ToListAsync()
            };

            return View("SystemOverview", dashboardViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class DashboardViewModel
    {
        public int TotalElderlyPeople { get; set; }
        public int TotalUsers { get; set; }
        public int TotalCarePlans { get; set; }
        public int PendingTasks { get; set; }
        public List<ActivityLog> RecentActivityLogs { get; set; } = new();
        public List<MedicationLog> RecentMedicationLogs { get; set; } = new();
        public List<AppointmentLog> UpcomingAppointments { get; set; } = new();
        public List<TaskSchedule> TodayTasks { get; set; } = new();
    }

    public class PatientDashboardViewModel
    {
        public ElderlyPerson Patient { get; set; } = null!;
        public int TotalCarePlans { get; set; }
        public int ActiveCarePlans { get; set; }
        public int PendingTasks { get; set; }
        public List<TaskSchedule> TodayTasks { get; set; } = new();
        public List<ActivityLog> RecentActivityLogs { get; set; } = new();
        public List<MedicationLog> RecentMedicationLogs { get; set; } = new();
        public List<AppointmentLog> UpcomingAppointments { get; set; } = new();
        public List<VitalSignsLog> RecentVitalSigns { get; set; } = new();
        public List<SymptomLog> RecentSymptoms { get; set; } = new();
    }
}
