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
            if (ModelState.IsValid)
            {
                medicationLog.CreatedAt = DateTime.Now;
                _context.Add(medicationLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
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
