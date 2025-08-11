using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElderlyCareApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ElderlyCareApp.Controllers
{
    public class CaregiverAssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CaregiverAssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CaregiverAssignments
        public async Task<IActionResult> Index()
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Only Family users can access caregiver assignments
            if (currentUserRole != "Family")
            {
                return Unauthorized("Access denied. Only family members can manage caregiver assignments.");
            }

            var assignments = await _context.CaregiverAssignments
                .Include(ca => ca.Caregiver)
                .Include(ca => ca.ElderlyPerson)
                .OrderByDescending(ca => ca.CreatedAt)
                .ToListAsync();

            return View(assignments);
        }

        // GET: CaregiverAssignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Only Family users can access caregiver assignments
            if (currentUserRole != "Family")
            {
                return Unauthorized("Access denied. Only family members can view caregiver assignments.");
            }

            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.CaregiverAssignments
                .Include(ca => ca.Caregiver)
                .Include(ca => ca.ElderlyPerson)
                .FirstOrDefaultAsync(ca => ca.Id == id);

            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // GET: CaregiverAssignments/Create
        public IActionResult Create()
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Only Family users can create caregiver assignments
            if (currentUserRole != "Family")
            {
                return Unauthorized("Access denied. Only family members can create caregiver assignments.");
            }

            // Get available caregivers (users with Caregiver role)
            var caregivers = _context.Users
                .Where(u => u.Role == UserRole.Caregiver && u.IsActive)
                .OrderBy(u => u.Name)
                .ToList();

            // Get available elderly people
            var elderlyPeople = _context.ElderlyPeople
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToList();

            ViewBag.Caregivers = caregivers;
            ViewBag.ElderlyPeople = elderlyPeople;

            return View();
        }

        // POST: CaregiverAssignments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaregiverId,ElderlyPersonId,StartDate,EndDate,Notes")] CaregiverAssignment assignment)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Only Family users can create caregiver assignments
            if (currentUserRole != "Family")
            {
                return Unauthorized("Access denied. Only family members can create caregiver assignments.");
            }

            if (ModelState.IsValid)
            {
                // Check if there's already an active assignment for this caregiver and patient
                var existingAssignment = await _context.CaregiverAssignments
                    .FirstOrDefaultAsync(ca => ca.CaregiverId == assignment.CaregiverId && 
                                             ca.ElderlyPersonId == assignment.ElderlyPersonId && 
                                             ca.IsActive);

                if (existingAssignment != null)
                {
                    ModelState.AddModelError("", "This caregiver is already assigned to this patient.");
                    
                    // Re-populate ViewBag for the form
                    var caregivers = _context.Users
                        .Where(u => u.Role == UserRole.Caregiver && u.IsActive)
                        .OrderBy(u => u.Name)
                        .ToList();
                    var elderlyPeople = _context.ElderlyPeople
                        .Where(p => p.IsActive)
                        .OrderBy(p => p.Name)
                        .ToList();
                    ViewBag.Caregivers = caregivers;
                    ViewBag.ElderlyPeople = elderlyPeople;
                    
                    return View(assignment);
                }

                assignment.CreatedAt = DateTime.Now;
                assignment.IsActive = true;

                _context.Add(assignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Re-populate ViewBag for the form
            var caregiversForForm = _context.Users
                .Where(u => u.Role == UserRole.Caregiver && u.IsActive)
                .OrderBy(u => u.Name)
                .ToList();
            var elderlyPeopleForForm = _context.ElderlyPeople
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToList();
            ViewBag.Caregivers = caregiversForForm;
            ViewBag.ElderlyPeople = elderlyPeopleForForm;

            return View(assignment);
        }

        // GET: CaregiverAssignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Only Family users can edit caregiver assignments
            if (currentUserRole != "Family")
            {
                return Unauthorized("Access denied. Only family members can edit caregiver assignments.");
            }

            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.CaregiverAssignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }

            // Get available caregivers and elderly people for the form
            var caregivers = _context.Users
                .Where(u => u.Role == UserRole.Caregiver && u.IsActive)
                .OrderBy(u => u.Name)
                .ToList();
            var elderlyPeople = _context.ElderlyPeople
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToList();

            ViewBag.Caregivers = caregivers;
            ViewBag.ElderlyPeople = elderlyPeople;

            return View(assignment);
        }

        // POST: CaregiverAssignments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CaregiverId,ElderlyPersonId,StartDate,EndDate,Notes,IsActive")] CaregiverAssignment assignment)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Only Family users can edit caregiver assignments
            if (currentUserRole != "Family")
            {
                return Unauthorized("Access denied. Only family members can edit caregiver assignments.");
            }

            if (id != assignment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if there's already an active assignment for this caregiver and patient (excluding current assignment)
                    var existingAssignment = await _context.CaregiverAssignments
                        .FirstOrDefaultAsync(ca => ca.CaregiverId == assignment.CaregiverId && 
                                                 ca.ElderlyPersonId == assignment.ElderlyPersonId && 
                                                 ca.IsActive && 
                                                 ca.Id != assignment.Id);

                    if (existingAssignment != null)
                    {
                        ModelState.AddModelError("", "This caregiver is already assigned to this patient.");
                        
                        // Re-populate ViewBag for the form
                        var caregivers = _context.Users
                            .Where(u => u.Role == UserRole.Caregiver && u.IsActive)
                            .OrderBy(u => u.Name)
                            .ToList();
                        var elderlyPeople = _context.ElderlyPeople
                            .Where(p => p.IsActive)
                            .OrderBy(p => p.Name)
                            .ToList();
                        ViewBag.Caregivers = caregivers;
                        ViewBag.ElderlyPeople = elderlyPeople;
                        
                        return View(assignment);
                    }

                    _context.Update(assignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentExists(assignment.Id))
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

            // Re-populate ViewBag for the form
            var caregiversForForm = _context.Users
                .Where(u => u.Role == UserRole.Caregiver && u.IsActive)
                .OrderBy(u => u.Name)
                .ToList();
            var elderlyPeopleForForm = _context.ElderlyPeople
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToList();
            ViewBag.Caregivers = caregiversForForm;
            ViewBag.ElderlyPeople = elderlyPeopleForForm;

            return View(assignment);
        }

        // GET: CaregiverAssignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Only Family users can delete caregiver assignments
            if (currentUserRole != "Family")
            {
                return Unauthorized("Access denied. Only family members can delete caregiver assignments.");
            }

            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.CaregiverAssignments
                .Include(ca => ca.Caregiver)
                .Include(ca => ca.ElderlyPerson)
                .FirstOrDefaultAsync(ca => ca.Id == id);

            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // POST: CaregiverAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Only Family users can delete caregiver assignments
            if (currentUserRole != "Family")
            {
                return Unauthorized("Access denied. Only family members can delete caregiver assignments.");
            }

            var assignment = await _context.CaregiverAssignments.FindAsync(id);
            if (assignment != null)
            {
                _context.CaregiverAssignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentExists(int id)
        {
            return _context.CaregiverAssignments.Any(e => e.Id == id);
        }
    }
}
