using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElderlyCareApp.Models;

namespace ElderlyCareApp.Controllers
{
    public class ActivityLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivityLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ActivityLogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ActivityLogs.Include(a => a.ElderlyPerson).Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ActivityLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityLog = await _context.ActivityLogs
                .Include(a => a.ElderlyPerson)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityLog == null)
            {
                return NotFound();
            }

            return View(activityLog);
        }

        // GET: ActivityLogs/Create
        public IActionResult Create()
        {
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // GET: ActivityLogs/TestDatabase
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                var elderlyCount = await _context.ElderlyPeople.CountAsync();
                var userCount = await _context.Users.CountAsync();
                var activityCount = await _context.ActivityLogs.CountAsync();
                
                return Content($"Database connection successful. ElderlyPeople: {elderlyCount}, Users: {userCount}, ActivityLogs: {activityCount}");
            }
            catch (Exception ex)
            {
                return Content($"Database connection failed: {ex.Message}");
            }
        }

        // POST: ActivityLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElderlyPersonId,UserId,ActivityType,Description,StartTime,EndTime,DurationMinutes,Notes")] ActivityLog activityLog)
        {
            // Add debugging information
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                // Log or handle validation errors
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            // Debug: Log the received data
            System.Diagnostics.Debug.WriteLine($"Received ActivityLog: ElderlyPersonId={activityLog.ElderlyPersonId}, UserId={activityLog.UserId}, ActivityType={activityLog.ActivityType}, StartTime={activityLog.StartTime}");

            try
            {
                if (ModelState.IsValid)
                {
                    // Ensure StartTime has a value
                    if (activityLog.StartTime == default(DateTime))
                    {
                        activityLog.StartTime = DateTime.Now;
                        System.Diagnostics.Debug.WriteLine($"StartTime was default, set to: {activityLog.StartTime}");
                    }
                    
                    activityLog.CreatedAt = DateTime.Now;
                    _context.Add(activityLog);
                    var result = await _context.SaveChangesAsync();
                    
                    System.Diagnostics.Debug.WriteLine($"SaveChangesAsync result: {result}");
                    
                    if (result > 0)
                    {
                        // Redirect to patient dashboard instead of index
                        return RedirectToAction("PatientDashboard", "Home", new { patientId = activityLog.ElderlyPersonId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to save activity to database");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ModelState is not valid");
                    var allErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    foreach (var error in allErrors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Validation error: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception occurred: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Error saving activity: {ex.Message}");
            }
            
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", activityLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", activityLog.UserId);
            return View(activityLog);
        }

        // GET: ActivityLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityLog = await _context.ActivityLogs.FindAsync(id);
            if (activityLog == null)
            {
                return NotFound();
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople.Where(p => p.IsActive), "Id", "Name", activityLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "Id", "Name", activityLog.UserId);
            return View(activityLog);
        }

        // POST: ActivityLogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElderlyPersonId,UserId,ActivityType,Description,StartTime,EndTime,DurationMinutes,Notes,CreatedAt")] ActivityLog activityLog)
        {
            if (id != activityLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityLog);
                    await _context.SaveChangesAsync();
                    // Redirect to patient dashboard instead of index
                    return RedirectToAction("PatientDashboard", "Home", new { patientId = activityLog.ElderlyPersonId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityLogExists(activityLog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", activityLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", activityLog.UserId);
            return View(activityLog);
        }

        // GET: ActivityLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityLog = await _context.ActivityLogs
                .Include(a => a.ElderlyPerson)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityLog == null)
            {
                return NotFound();
            }

            return View(activityLog);
        }

        // POST: ActivityLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityLog = await _context.ActivityLogs.FindAsync(id);
            if (activityLog != null)
            {
                _context.ActivityLogs.Remove(activityLog);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityLogExists(int id)
        {
            return _context.ActivityLogs.Any(e => e.Id == id);
        }
    }
}
