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
    public class VitalSignsLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VitalSignsLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VitalSignsLogs
        public async Task<IActionResult> Index()
        {
            var vitalSignsLogs = await _context.VitalSignsLogs
                .Include(v => v.ElderlyPerson)
                .Include(v => v.User)
                .OrderByDescending(v => v.MeasurementTime)
                .ToListAsync();
            return View(vitalSignsLogs);
        }

        // GET: VitalSignsLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vitalSignsLog = await _context.VitalSignsLogs
                .Include(v => v.ElderlyPerson)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vitalSignsLog == null)
            {
                return NotFound();
            }

            return View(vitalSignsLog);
        }

        // GET: VitalSignsLogs/Create
        public IActionResult Create()
        {
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: VitalSignsLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElderlyPersonId,UserId,MeasurementTime,BloodPressure,HeartRate,Temperature,RespiratoryRate,OxygenSaturation,Notes")] VitalSignsLog vitalSignsLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vitalSignsLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", vitalSignsLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", vitalSignsLog.UserId);
            return View(vitalSignsLog);
        }

        // GET: VitalSignsLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vitalSignsLog = await _context.VitalSignsLogs.FindAsync(id);
            if (vitalSignsLog == null)
            {
                return NotFound();
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", vitalSignsLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", vitalSignsLog.UserId);
            return View(vitalSignsLog);
        }

        // POST: VitalSignsLogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElderlyPersonId,UserId,MeasurementTime,BloodPressure,HeartRate,Temperature,RespiratoryRate,OxygenSaturation,Notes")] VitalSignsLog vitalSignsLog)
        {
            if (id != vitalSignsLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vitalSignsLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VitalSignsLogExists(vitalSignsLog.Id))
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
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", vitalSignsLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", vitalSignsLog.UserId);
            return View(vitalSignsLog);
        }

        // GET: VitalSignsLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vitalSignsLog = await _context.VitalSignsLogs
                .Include(v => v.ElderlyPerson)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vitalSignsLog == null)
            {
                return NotFound();
            }

            return View(vitalSignsLog);
        }

        // POST: VitalSignsLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vitalSignsLog = await _context.VitalSignsLogs.FindAsync(id);
            if (vitalSignsLog != null)
            {
                _context.VitalSignsLogs.Remove(vitalSignsLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VitalSignsLogExists(int id)
        {
            return _context.VitalSignsLogs.Any(e => e.Id == id);
        }
    }
} 