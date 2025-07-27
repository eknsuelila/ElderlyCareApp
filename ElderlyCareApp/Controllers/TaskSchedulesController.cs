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
    public class TaskSchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskSchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TaskSchedules
        public async Task<IActionResult> Index()
        {
            var taskSchedules = await _context.TaskSchedules
                .Include(t => t.ElderlyPerson)
                .Include(t => t.User)
                .Include(t => t.CarePlan)
                .OrderBy(t => t.ScheduledTime)
                .ToListAsync();
            return View(taskSchedules);
        }

        // GET: TaskSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskSchedule = await _context.TaskSchedules
                .Include(t => t.ElderlyPerson)
                .Include(t => t.User)
                .Include(t => t.CarePlan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskSchedule == null)
            {
                return NotFound();
            }

            return View(taskSchedule);
        }

        // GET: TaskSchedules/Create
        public IActionResult Create()
        {
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            ViewData["CarePlanId"] = new SelectList(_context.CarePlans, "Id", "Title");
            return View();
        }

        // POST: TaskSchedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElderlyPersonId,UserId,CarePlanId,Title,Description,TaskType,ScheduledTime,DurationMinutes,Priority,Status,Location,Instructions,Requirements,Notes,IsRecurring,RecurrenceType,RecurrenceInterval,RecurrenceDays,RecurrenceEndDate,MaxOccurrences,HasReminder,ReminderMinutesBefore")] TaskSchedule taskSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", taskSchedule.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", taskSchedule.UserId);
            ViewData["CarePlanId"] = new SelectList(_context.CarePlans, "Id", "Title", taskSchedule.CarePlanId);
            return View(taskSchedule);
        }

        // GET: TaskSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskSchedule = await _context.TaskSchedules.FindAsync(id);
            if (taskSchedule == null)
            {
                return NotFound();
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", taskSchedule.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", taskSchedule.UserId);
            ViewData["CarePlanId"] = new SelectList(_context.CarePlans, "Id", "Title", taskSchedule.CarePlanId);
            return View(taskSchedule);
        }

        // POST: TaskSchedules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElderlyPersonId,UserId,CarePlanId,Title,Description,TaskType,ScheduledTime,DurationMinutes,Priority,Status,Location,Instructions,Requirements,Notes,IsRecurring,RecurrenceType,RecurrenceInterval,RecurrenceDays,RecurrenceEndDate,MaxOccurrences,HasReminder,ReminderMinutesBefore")] TaskSchedule taskSchedule)
        {
            if (id != taskSchedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskScheduleExists(taskSchedule.Id))
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
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", taskSchedule.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", taskSchedule.UserId);
            ViewData["CarePlanId"] = new SelectList(_context.CarePlans, "Id", "Title", taskSchedule.CarePlanId);
            return View(taskSchedule);
        }

        // GET: TaskSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskSchedule = await _context.TaskSchedules
                .Include(t => t.ElderlyPerson)
                .Include(t => t.User)
                .Include(t => t.CarePlan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskSchedule == null)
            {
                return NotFound();
            }

            return View(taskSchedule);
        }

        // POST: TaskSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskSchedule = await _context.TaskSchedules.FindAsync(id);
            if (taskSchedule != null)
            {
                _context.TaskSchedules.Remove(taskSchedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskScheduleExists(int id)
        {
            return _context.TaskSchedules.Any(e => e.Id == id);
        }
    }
} 