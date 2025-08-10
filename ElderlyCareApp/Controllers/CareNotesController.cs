using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElderlyCareApp.Models;
using System.Threading.Tasks;

namespace ElderlyCareApp.Controllers
{
    public class CareNotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CareNotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CareNotes
        public async Task<IActionResult> Index()
        {
            var careNotes = await _context.CareNotes
                .Include(c => c.ElderlyPerson)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(careNotes);
        }

        // GET: CareNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var careNote = await _context.CareNotes
                .Include(c => c.ElderlyPerson)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (careNote == null)
            {
                return NotFound();
            }

            return View(careNote);
        }

        // GET: CareNotes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople.Where(p => p.IsActive), "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "Id", "Name");
            return View();
        }

        // POST: CareNotes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElderlyPersonId,UserId,NoteType,Title,Content")] CareNote careNote)
        {
            // Add debugging information
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            // Debug: Log the received data
            System.Diagnostics.Debug.WriteLine($"Received CareNote: ElderlyPersonId={careNote.ElderlyPersonId}, UserId={careNote.UserId}, NoteType={careNote.NoteType}, Title={careNote.Title}");

            try
            {
                if (ModelState.IsValid)
                {
                    careNote.CreatedAt = DateTime.Now;
                    _context.Add(careNote);
                    var result = await _context.SaveChangesAsync();
                    
                    System.Diagnostics.Debug.WriteLine($"SaveChangesAsync result: {result}");
                    
                    if (result > 0)
                    {
                        // Redirect to patient dashboard instead of index
                        return RedirectToAction("PatientDashboard", "Home", new { patientId = careNote.ElderlyPersonId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to save care note to database");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ModelState is not valid");
                    var allErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    foreach (var error in allErrors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Validation error: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception occurred: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Error saving care note: {ex.Message}");
            }
            
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople.Where(p => p.IsActive), "Id", "Name", careNote.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "Id", "Name", careNote.UserId);
            return View(careNote);
        }

        // GET: CareNotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var careNote = await _context.CareNotes.FindAsync(id);
            if (careNote == null)
            {
                return NotFound();
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople.Where(p => p.IsActive), "Id", "Name", careNote.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "Id", "Name", careNote.UserId);
            return View(careNote);
        }

        // POST: CareNotes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElderlyPersonId,UserId,Title,Content,Notes")] CareNote careNote)
        {
            if (id != careNote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(careNote);
                    await _context.SaveChangesAsync();
                    // Redirect to patient dashboard instead of index
                    return RedirectToAction("PatientDashboard", "Home", new { patientId = careNote.ElderlyPersonId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CareNoteExists(careNote.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople.Where(p => p.IsActive), "Id", "Name", careNote.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "Id", "Name", careNote.UserId);
            return View(careNote);
        }

        // GET: CareNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var careNote = await _context.CareNotes
                .Include(c => c.ElderlyPerson)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (careNote == null)
            {
                return NotFound();
            }

            return View(careNote);
        }

        // POST: CareNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var careNote = await _context.CareNotes.FindAsync(id);
            if (careNote != null)
            {
                _context.CareNotes.Remove(careNote);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CareNoteExists(int id)
        {
            return _context.CareNotes.Any(e => e.Id == id);
        }
    }
} 