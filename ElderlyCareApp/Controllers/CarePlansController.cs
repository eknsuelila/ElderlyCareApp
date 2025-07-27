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
    public class CarePlansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarePlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarePlans
        public async Task<IActionResult> Index()
        {
            var carePlans = await _context.CarePlans
                .Include(c => c.ElderlyPerson)
                .Include(c => c.User)
                .Include(c => c.TaskSchedules)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(carePlans);
        }

        // GET: CarePlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carePlan = await _context.CarePlans
                .Include(c => c.ElderlyPerson)
                .Include(c => c.User)
                .Include(c => c.TaskSchedules)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carePlan == null)
            {
                return NotFound();
            }

            return View(carePlan);
        }

        // GET: CarePlans/Create
        public IActionResult Create()
        {
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: CarePlans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElderlyPersonId,UserId,Title,Description,Goals,Interventions,StartDate,EndDate,Status,Priority")] CarePlan carePlan)
        {
            if (ModelState.IsValid)
            {
                carePlan.CreatedAt = DateTime.Now;
                _context.Add(carePlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", carePlan.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", carePlan.UserId);
            return View(carePlan);
        }

        // GET: CarePlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carePlan = await _context.CarePlans.FindAsync(id);
            if (carePlan == null)
            {
                return NotFound();
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", carePlan.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", carePlan.UserId);
            return View(carePlan);
        }

        // POST: CarePlans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElderlyPersonId,UserId,Title,Description,Goals,Interventions,StartDate,EndDate,Status,Priority,CreatedAt")] CarePlan carePlan)
        {
            if (id != carePlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carePlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarePlanExists(carePlan.Id))
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
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", carePlan.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", carePlan.UserId);
            return View(carePlan);
        }

        // GET: CarePlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carePlan = await _context.CarePlans
                .Include(c => c.ElderlyPerson)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carePlan == null)
            {
                return NotFound();
            }

            return View(carePlan);
        }

        // POST: CarePlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carePlan = await _context.CarePlans.FindAsync(id);
            if (carePlan != null)
            {
                _context.CarePlans.Remove(carePlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarePlanExists(int id)
        {
            return _context.CarePlans.Any(e => e.Id == id);
        }
    }
} 