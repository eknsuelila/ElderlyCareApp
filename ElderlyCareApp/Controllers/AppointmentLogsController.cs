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
    public class AppointmentLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AppointmentLogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AppointmentLogs.Include(a => a.ElderlyPerson).Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AppointmentLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentLog = await _context.AppointmentLogs
                .Include(a => a.ElderlyPerson)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentLog == null)
            {
                return NotFound();
            }

            return View(appointmentLog);
        }

        // GET: AppointmentLogs/Create
        public IActionResult Create()
        {
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: AppointmentLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElderlyPersonId,UserId,AppointmentType,Title,Description,ProviderName,Location,ScheduledDateTime,Status,Notes")] AppointmentLog appointmentLog)
        {
            // Add debugging information
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            // Debug: Log the received data
            System.Diagnostics.Debug.WriteLine($"Received AppointmentLog: ElderlyPersonId={appointmentLog.ElderlyPersonId}, UserId={appointmentLog.UserId}, AppointmentType={appointmentLog.AppointmentType}, ScheduledDateTime={appointmentLog.ScheduledDateTime}");

            try
            {
                if (ModelState.IsValid)
                {
                    // Ensure ScheduledDateTime has a value
                    if (appointmentLog.ScheduledDateTime == default(DateTime))
                    {
                        appointmentLog.ScheduledDateTime = DateTime.Now;
                        System.Diagnostics.Debug.WriteLine($"ScheduledDateTime was default, set to: {appointmentLog.ScheduledDateTime}");
                    }
                    
                    appointmentLog.CreatedAt = DateTime.Now;
                    _context.Add(appointmentLog);
                    var result = await _context.SaveChangesAsync();
                    
                    System.Diagnostics.Debug.WriteLine($"SaveChangesAsync result: {result}");
                    
                    if (result > 0)
                    {
                        // Redirect to patient dashboard instead of index
                        return RedirectToAction("PatientDashboard", "Home", new { patientId = appointmentLog.ElderlyPersonId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to save appointment log to database");
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
                ModelState.AddModelError("", $"Error saving appointment log: {ex.Message}");
            }
            
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", appointmentLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", appointmentLog.UserId);
            return View(appointmentLog);
        }

        // GET: AppointmentLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentLog = await _context.AppointmentLogs.FindAsync(id);
            if (appointmentLog == null)
            {
                return NotFound();
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople.Where(p => p.IsActive), "Id", "Name", appointmentLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "Id", "Name", appointmentLog.UserId);
            return View(appointmentLog);
        }

        // POST: AppointmentLogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElderlyPersonId,UserId,Title,Description,AppointmentType,ScheduledDateTime,Status,ProviderName,Location,Notes")] AppointmentLog appointmentLog)
        {
            if (id != appointmentLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentLog);
                    await _context.SaveChangesAsync();
                    // Redirect to patient dashboard instead of index
                    return RedirectToAction("PatientDashboard", "Home", new { patientId = appointmentLog.ElderlyPersonId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentLogExists(appointmentLog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople.Where(p => p.IsActive), "Id", "Name", appointmentLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "Id", "Name", appointmentLog.UserId);
            return View(appointmentLog);
        }

        // GET: AppointmentLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentLog = await _context.AppointmentLogs
                .Include(a => a.ElderlyPerson)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentLog == null)
            {
                return NotFound();
            }

            return View(appointmentLog);
        }

        // POST: AppointmentLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentLog = await _context.AppointmentLogs.FindAsync(id);
            if (appointmentLog != null)
            {
                _context.AppointmentLogs.Remove(appointmentLog);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentLogExists(int id)
        {
            return _context.AppointmentLogs.Any(e => e.Id == id);
        }
    }
} 