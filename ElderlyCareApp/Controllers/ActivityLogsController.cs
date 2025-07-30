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

        // POST: ActivityLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElderlyPersonId,UserId,ActivityType,Description,StartTime,EndTime,DurationMinutes,Notes")] ActivityLog activityLog)
        {
            if (ModelState.IsValid)
            {
                activityLog.CreatedAt = DateTime.Now;
                _context.Add(activityLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElderlyPersonId,UserId,ActivityType,Description,StartTime,EndTime")] ActivityLog activityLog)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople.Where(p => p.IsActive), "Id", "Name", activityLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "Id", "Name", activityLog.UserId);
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
