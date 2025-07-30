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
            if (ModelState.IsValid)
            {
                careNote.CreatedAt = DateTime.Now;
                _context.Add(careNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElderlyPersonId,UserId,NoteType,Title,Content,CreatedAt")] CareNote careNote)
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
                return RedirectToAction(nameof(Index));
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