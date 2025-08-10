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
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // All users can view the list
            return View(await _context.ElderlyPeople.ToListAsync());
        }

        // GET: ElderlyPeople/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // All users can view details
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
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Only Family users can create new elderly people
            if (currentUserRole == "Caregiver")
            {
                return Unauthorized("Access denied. Caregivers cannot create new elderly people.");
            }
            
            return View();
        }

        // POST: ElderlyPeople/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,DateOfBirth,PhoneNumber,EmergencyContactName,EmergencyContactPhone,Allergies,MedicalConditions,Notes,IsActive")] ElderlyPerson elderlyPerson)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Only Family users can create new elderly people
            if (currentUserRole == "Caregiver")
            {
                return Unauthorized("Access denied. Caregivers cannot create new elderly people.");
            }
            
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
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // All users can edit elderly people
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
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // All users can edit elderly people
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
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Caregivers cannot delete elderly people
            if (currentUserRole == "Caregiver")
            {
                return Unauthorized("Access denied. Caregivers cannot delete elderly people.");
            }
            
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
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Caregivers cannot delete elderly people
            if (currentUserRole == "Caregiver")
            {
                return Unauthorized("Access denied. Caregivers cannot delete elderly people.");
            }
            
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
