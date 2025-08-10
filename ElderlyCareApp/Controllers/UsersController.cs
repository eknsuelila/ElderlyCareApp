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
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // All users can view the list
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // All users can view details
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Only Family users can create new users
            if (currentUserRole == "Caregiver")
            {
                return Unauthorized("Access denied. Caregivers cannot create new users.");
            }
            
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Password,PhoneNumber,Role,IsActive")] User user)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Only Family users can create new users
            if (currentUserRole == "Caregiver")
            {
                return Unauthorized("Access denied. Caregivers cannot create new users.");
            }
            
            if (ModelState.IsValid)
            {
                user.CreatedAt = DateTime.Now;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // All users can edit user information
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,PhoneNumber,Role,IsActive,LastLoginAt,CreatedAt")] User user)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // All users can edit user information
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Caregivers cannot delete users
            if (currentUserRole == "Caregiver")
            {
                return Unauthorized("Access denied. Caregivers cannot delete users.");
            }
            
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUserRole = HttpContext.Session.GetString("CurrentUserRole");
            
            // Caregivers cannot delete users
            if (currentUserRole == "Caregiver")
            {
                return Unauthorized("Access denied. Caregivers cannot delete users.");
            }
            
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
} 