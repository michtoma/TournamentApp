using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mundial.Data;
using Mundial.Models;

namespace Mundial.Controllers
{
    public class GameCountriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameCountriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GameCountries
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GameCountry.Include(g => g.Country).Include(g => g.Game);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GameCountries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GameCountry == null)
            {
                return NotFound();
            }

            var gameCountry = await _context.GameCountry
                .Include(g => g.Country)
                .Include(g => g.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameCountry == null)
            {
                return NotFound();
            }

            return View(gameCountry);
        }

        // GET: GameCountries/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Id");
            return View();
        }

        // POST: GameCountries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CountryId,GameId")] GameCountry gameCountry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameCountry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", gameCountry.CountryId);
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Id", gameCountry.GameId);
            return View(gameCountry);
        }

        // GET: GameCountries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GameCountry == null)
            {
                return NotFound();
            }

            var gameCountry = await _context.GameCountry.FindAsync(id);
            if (gameCountry == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", gameCountry.CountryId);
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Id", gameCountry.GameId);
            return View(gameCountry);
        }

        // POST: GameCountries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CountryId,GameId")] GameCountry gameCountry)
        {
            if (id != gameCountry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameCountry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameCountryExists(gameCountry.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", gameCountry.CountryId);
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Id", gameCountry.GameId);
            return View(gameCountry);
        }

        // GET: GameCountries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GameCountry == null)
            {
                return NotFound();
            }

            var gameCountry = await _context.GameCountry
                .Include(g => g.Country)
                .Include(g => g.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameCountry == null)
            {
                return NotFound();
            }

            return View(gameCountry);
        }

        // POST: GameCountries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GameCountry == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GameCountry'  is null.");
            }
            var gameCountry = await _context.GameCountry.FindAsync(id);
            if (gameCountry != null)
            {
                _context.GameCountry.Remove(gameCountry);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameCountryExists(int id)
        {
          return _context.GameCountry.Any(e => e.Id == id);
        }
    }
}
