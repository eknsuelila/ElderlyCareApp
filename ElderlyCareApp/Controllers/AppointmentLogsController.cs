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
            var appointmentLogs = await _context.AppointmentLogs
                .Include(a => a.ElderlyPerson)
                .Include(a => a.User)
                .OrderByDescending(a => a.ScheduledDateTime)
                .ToListAsync();
            return View(appointmentLogs);
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
            if (ModelState.IsValid)
            {
                _context.Add(appointmentLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", appointmentLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", appointmentLog.UserId);
            return View(appointmentLog);
        }

        // POST: AppointmentLogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElderlyPersonId,UserId,AppointmentType,Title,Description,ProviderName,Location,ScheduledDateTime,Status,Notes")] AppointmentLog appointmentLog)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", appointmentLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", appointmentLog.UserId);
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
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentLogExists(int id)
        {
            return _context.AppointmentLogs.Any(e => e.Id == id);
        }
    }
} 