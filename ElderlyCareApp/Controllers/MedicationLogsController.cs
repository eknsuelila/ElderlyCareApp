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
    public class MedicationLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicationLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MedicationLogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MedicationLogs.Include(m => m.ElderlyPerson).Include(m => m.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MedicationLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationLog = await _context.MedicationLogs
                .Include(m => m.ElderlyPerson)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicationLog == null)
            {
                return NotFound();
            }

            return View(medicationLog);
        }

        // GET: MedicationLogs/Create
        public IActionResult Create()
        {
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: MedicationLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElderlyPersonId,UserId,MedicationName,Dosage,Taken,Timestamp,Notes")] MedicationLog medicationLog)
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
            System.Diagnostics.Debug.WriteLine($"Received MedicationLog: ElderlyPersonId={medicationLog.ElderlyPersonId}, UserId={medicationLog.UserId}, MedicationName={medicationLog.MedicationName}, Timestamp={medicationLog.Timestamp}");

            try
            {
                if (ModelState.IsValid)
                {
                    // Ensure Timestamp has a value
                    if (medicationLog.Timestamp == default(DateTime))
                    {
                        medicationLog.Timestamp = DateTime.Now;
                        System.Diagnostics.Debug.WriteLine($"Timestamp was default, set to: {medicationLog.Timestamp}");
                    }
                    
                    medicationLog.CreatedAt = DateTime.Now;
                    _context.Add(medicationLog);
                    var result = await _context.SaveChangesAsync();
                    
                    System.Diagnostics.Debug.WriteLine($"SaveChangesAsync result: {result}");
                    
                    if (result > 0)
                    {
                        // Redirect to patient dashboard instead of index
                        return RedirectToAction("PatientDashboard", "Home", new { patientId = medicationLog.ElderlyPersonId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to save medication log to database");
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
                ModelState.AddModelError("", $"Error saving medication log: {ex.Message}");
            }
            
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", medicationLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", medicationLog.UserId);
            return View(medicationLog);
        }

        // GET: MedicationLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationLog = await _context.MedicationLogs.FindAsync(id);
            if (medicationLog == null)
            {
                return NotFound();
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople.Where(p => p.IsActive), "Id", "Name", medicationLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "Id", "Name", medicationLog.UserId);
            return View(medicationLog);
        }

        // POST: MedicationLogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElderlyPersonId,UserId,MedicationName,Dosage,Taken,Timestamp,Notes")] MedicationLog medicationLog)
        {
            if (id != medicationLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationLog);
                    await _context.SaveChangesAsync();
                    // Redirect to patient dashboard instead of index
                    return RedirectToAction("PatientDashboard", "Home", new { patientId = medicationLog.ElderlyPersonId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationLogExists(medicationLog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople.Where(p => p.IsActive), "Id", "Name", medicationLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "Id", "Name", medicationLog.UserId);
            return View(medicationLog);
        }

        // GET: MedicationLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationLog = await _context.MedicationLogs
                .Include(m => m.ElderlyPerson)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicationLog == null)
            {
                return NotFound();
            }

            return View(medicationLog);
        }

        // POST: MedicationLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicationLog = await _context.MedicationLogs.FindAsync(id);
            if (medicationLog != null)
            {
                _context.MedicationLogs.Remove(medicationLog);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationLogExists(int id)
        {
            return _context.MedicationLogs.Any(e => e.Id == id);
        }
    }
}
