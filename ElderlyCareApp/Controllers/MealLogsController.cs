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
    public class MealLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MealLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MealLogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MealLogs.Include(m => m.ElderlyPerson).Include(m => m.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MealLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealLog = await _context.MealLogs
                .Include(m => m.ElderlyPerson)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mealLog == null)
            {
                return NotFound();
            }

            return View(mealLog);
        }

        // GET: MealLogs/Create
        public IActionResult Create()
        {
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: MealLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElderlyPersonId,UserId,MealType,MealName,Description,MealTime,Notes")] MealLog mealLog)
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
            System.Diagnostics.Debug.WriteLine($"Received MealLog: ElderlyPersonId={mealLog.ElderlyPersonId}, UserId={mealLog.UserId}, MealType={mealLog.MealType}, MealTime={mealLog.MealTime}");

            try
            {
                if (ModelState.IsValid)
                {
                    // Ensure MealTime has a value
                    if (mealLog.MealTime == default(DateTime))
                    {
                        mealLog.MealTime = DateTime.Now;
                        System.Diagnostics.Debug.WriteLine($"MealTime was default, set to: {mealLog.MealTime}");
                    }
                    
                    mealLog.CreatedAt = DateTime.Now;
                    _context.Add(mealLog);
                    var result = await _context.SaveChangesAsync();
                    
                    System.Diagnostics.Debug.WriteLine($"SaveChangesAsync result: {result}");
                    
                    if (result > 0)
                    {
                        // Redirect to patient dashboard instead of index
                        return RedirectToAction("PatientDashboard", "Home", new { patientId = mealLog.ElderlyPersonId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to save meal log to database");
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
                ModelState.AddModelError("", $"Error saving meal log: {ex.Message}");
            }
            
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", mealLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", mealLog.UserId);
            return View(mealLog);
        }

        // GET: MealLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealLog = await _context.MealLogs.FindAsync(id);
            if (mealLog == null)
            {
                return NotFound();
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople.Where(p => p.IsActive), "Id", "Name", mealLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "Id", "Name", mealLog.UserId);
            return View(mealLog);
        }

        // POST: MealLogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElderlyPersonId,UserId,MealType,Description,MealTime,Notes")] MealLog mealLog)
        {
            if (id != mealLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mealLog);
                    await _context.SaveChangesAsync();
                    // Redirect to patient dashboard instead of index
                    return RedirectToAction("PatientDashboard", "Home", new { patientId = mealLog.ElderlyPersonId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealLogExists(mealLog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople.Where(p => p.IsActive), "Id", "Name", mealLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "Id", "Name", mealLog.UserId);
            return View(mealLog);
        }

        // GET: MealLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealLog = await _context.MealLogs
                .Include(m => m.ElderlyPerson)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mealLog == null)
            {
                return NotFound();
            }

            return View(mealLog);
        }

        // POST: MealLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mealLog = await _context.MealLogs.FindAsync(id);
            if (mealLog != null)
            {
                _context.MealLogs.Remove(mealLog);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MealLogExists(int id)
        {
            return _context.MealLogs.Any(e => e.Id == id);
        }
    }
}
