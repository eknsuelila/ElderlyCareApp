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
    public class SocialInteractionLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SocialInteractionLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SocialInteractionLogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SocialInteractionLogs.Include(s => s.ElderlyPerson).Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SocialInteractionLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialInteractionLog = await _context.SocialInteractionLogs
                .Include(s => s.ElderlyPerson)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (socialInteractionLog == null)
            {
                return NotFound();
            }

            return View(socialInteractionLog);
        }

        // GET: SocialInteractionLogs/Create
        public IActionResult Create()
        {
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: SocialInteractionLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ElderlyPersonId,UserId,InteractionType,Description,Timestamp")] SocialInteractionLog socialInteractionLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(socialInteractionLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Id", socialInteractionLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", socialInteractionLog.UserId);
            return View(socialInteractionLog);
        }

        // GET: SocialInteractionLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialInteractionLog = await _context.SocialInteractionLogs.FindAsync(id);
            if (socialInteractionLog == null)
            {
                return NotFound();
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Id", socialInteractionLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", socialInteractionLog.UserId);
            return View(socialInteractionLog);
        }

        // POST: SocialInteractionLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElderlyPersonId,UserId,InteractionType,Description,Timestamp")] SocialInteractionLog socialInteractionLog)
        {
            if (id != socialInteractionLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(socialInteractionLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SocialInteractionLogExists(socialInteractionLog.Id))
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
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Id", socialInteractionLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", socialInteractionLog.UserId);
            return View(socialInteractionLog);
        }

        // GET: SocialInteractionLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialInteractionLog = await _context.SocialInteractionLogs
                .Include(s => s.ElderlyPerson)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (socialInteractionLog == null)
            {
                return NotFound();
            }

            return View(socialInteractionLog);
        }

        // POST: SocialInteractionLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var socialInteractionLog = await _context.SocialInteractionLogs.FindAsync(id);
            if (socialInteractionLog != null)
            {
                _context.SocialInteractionLogs.Remove(socialInteractionLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SocialInteractionLogExists(int id)
        {
            return _context.SocialInteractionLogs.Any(e => e.Id == id);
        }
    }
}
