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
    public class ElderlyPeopleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ElderlyPeopleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ElderlyPeople
        public async Task<IActionResult> Index()
        {
            return View(await _context.ElderlyPeople.ToListAsync());
        }

        // GET: ElderlyPeople/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elderlyPerson = await _context.ElderlyPeople
                .FirstOrDefaultAsync(m => m.Id == id);
            if (elderlyPerson == null)
            {
                return NotFound();
            }

            return View(elderlyPerson);
        }

        // GET: ElderlyPeople/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ElderlyPeople/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,DateOfBirth,PhoneNumber,EmergencyContactName,EmergencyContactPhone,Allergies,MedicalConditions,Notes,IsActive")] ElderlyPerson elderlyPerson)
        {
            if (ModelState.IsValid)
            {
                elderlyPerson.CreatedAt = DateTime.Now;
                _context.Add(elderlyPerson);
                await _context.SaveChangesAsync();
                return RedirectToAction("PatientDashboard", "Home", new { patientId = elderlyPerson.Id });
            }
            return View(elderlyPerson);
        }

        // GET: ElderlyPeople/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elderlyPerson = await _context.ElderlyPeople.FindAsync(id);
            if (elderlyPerson == null)
            {
                return NotFound();
            }
            return View(elderlyPerson);
        }

        // POST: ElderlyPeople/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBirth,PhoneNumber,EmergencyContactName,EmergencyContactPhone,Allergies,MedicalConditions,Notes,IsActive,CreatedAt")] ElderlyPerson elderlyPerson)
        {
            if (id != elderlyPerson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(elderlyPerson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElderlyPersonExists(elderlyPerson.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("PatientDashboard", "Home", new { patientId = elderlyPerson.Id });
            }
            return View(elderlyPerson);
        }

        // GET: ElderlyPeople/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elderlyPerson = await _context.ElderlyPeople
                .FirstOrDefaultAsync(m => m.Id == id);
            if (elderlyPerson == null)
            {
                return NotFound();
            }

            return View(elderlyPerson);
        }

        // POST: ElderlyPeople/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var elderlyPerson = await _context.ElderlyPeople.FindAsync(id);
            if (elderlyPerson != null)
            {
                _context.ElderlyPeople.Remove(elderlyPerson);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ElderlyPersonExists(int id)
        {
            return _context.ElderlyPeople.Any(e => e.Id == id);
        }
    }
}
