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
            if (ModelState.IsValid)
            {
                mealLog.CreatedAt = DateTime.Now;
                _context.Add(mealLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
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
