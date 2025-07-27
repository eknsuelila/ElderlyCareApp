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
    public class SymptomLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SymptomLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SymptomLogs
        public async Task<IActionResult> Index()
        {
            var symptomLogs = await _context.SymptomLogs
                .Include(s => s.ElderlyPerson)
                .Include(s => s.User)
                .OrderByDescending(s => s.LogTime)
                .ToListAsync();
            return View(symptomLogs);
        }

        // GET: SymptomLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var symptomLog = await _context.SymptomLogs
                .Include(s => s.ElderlyPerson)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (symptomLog == null)
            {
                return NotFound();
            }

            return View(symptomLog);
        }

        // GET: SymptomLogs/Create
        public IActionResult Create()
        {
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: SymptomLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElderlyPersonId,UserId,LogTime,Symptom,Description,Severity,PainLevel,Location,Triggers,ReliefMethods,IsOngoing,Mood,AnxietyLevel,DepressionLevel,SleepQuality,HoursSlept,CognitiveSymptoms,MemoryIssues,Confusion,PhysicalSymptoms,MobilityIssues,BalanceIssues,DigestiveIssues,RespiratoryIssues,CardiovascularIssues,MedicationSideEffects,MedicationCompliance,Notes,RequiresMedicalAttention,MedicalActionTaken")] SymptomLog symptomLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(symptomLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", symptomLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", symptomLog.UserId);
            return View(symptomLog);
        }

        // GET: SymptomLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var symptomLog = await _context.SymptomLogs.FindAsync(id);
            if (symptomLog == null)
            {
                return NotFound();
            }
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", symptomLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", symptomLog.UserId);
            return View(symptomLog);
        }

        // POST: SymptomLogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElderlyPersonId,UserId,LogTime,Symptom,Description,Severity,PainLevel,Location,Triggers,ReliefMethods,IsOngoing,Mood,AnxietyLevel,DepressionLevel,SleepQuality,HoursSlept,CognitiveSymptoms,MemoryIssues,Confusion,PhysicalSymptoms,MobilityIssues,BalanceIssues,DigestiveIssues,RespiratoryIssues,CardiovascularIssues,MedicationSideEffects,MedicationCompliance,Notes,RequiresMedicalAttention,MedicalActionTaken")] SymptomLog symptomLog)
        {
            if (id != symptomLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(symptomLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SymptomLogExists(symptomLog.Id))
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
            ViewData["ElderlyPersonId"] = new SelectList(_context.ElderlyPeople, "Id", "Name", symptomLog.ElderlyPersonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", symptomLog.UserId);
            return View(symptomLog);
        }

        // GET: SymptomLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var symptomLog = await _context.SymptomLogs
                .Include(s => s.ElderlyPerson)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (symptomLog == null)
            {
                return NotFound();
            }

            return View(symptomLog);
        }

        // POST: SymptomLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var symptomLog = await _context.SymptomLogs.FindAsync(id);
            if (symptomLog != null)
            {
                _context.SymptomLogs.Remove(symptomLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SymptomLogExists(int id)
        {
            return _context.SymptomLogs.Any(e => e.Id == id);
        }
    }
} 