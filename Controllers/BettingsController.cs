using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mundial.Data;
using Mundial.Models;
using Mundial.ViewModels;

namespace Mundial.Controllers
{
    public class BettingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUsers> _userManager;

        public BettingsController(ApplicationDbContext context, UserManager<AppUsers> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Bettings
        public async Task<IActionResult> Index()
        {
            List<Game> games = _context.Game
                .Include(g => g.gameCountries)
                .ThenInclude(c => c.Country).ToList();
            string user = _userManager.GetUserId(User);
            List<Bettings> bets = _context.Bettings
                .Include(g => g.Game)
                .ThenInclude(c => c.gameCountries).ThenInclude(c => c.Country).Where(b => b.UserID == user).ToList();
            BettingsViewModel viewModel = new();
            viewModel.UserId = user;
            viewModel.Games = games;
            viewModel.Betts = bets;
            return View(viewModel);
        }

        // GET: Bettings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bettings == null)
            {
                return NotFound();
            }

            var bettings = await _context.Bettings
                .Include(b => b.Game)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bettings == null)
            {
                return NotFound();
            }

            return View(bettings);
        }

        // GET: Bettings/Create
        public IActionResult Create()
        {
            string userID = _userManager.GetUserId(User);
            var games = _context.Game.Include(g => g.gameCountries).ThenInclude(g => g.Country).ToList();
            BettingsViewModel viewModel = new();
            viewModel.UserId = userID;
            viewModel.Games = games;
            var tekst = "";
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var game in games)
            {
                if (game.gameCountries != null)
                {
                    tekst = game.DateTime.ToString() + " ";
                    foreach (var country in game.gameCountries)
                    {
                        tekst += country.Country.Name + " ";
                    }

                }
                selectList.Add(new SelectListItem()
                {
                    Value = game.Id.ToString(),
                    Text = tekst
                });
            }
            viewModel.GamesSelected = selectList;
            return View(viewModel);
        }

        // POST: Bettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( BettingsViewModel bettings)
        {
            if (ModelState.IsValid)
            {
                string userID = _userManager.GetUserId(User);
                Bettings bett = new Bettings();
                bett.GameId=bettings.GameId;
                bett.UserID = userID;
                bett.ScoreTeam1 = bettings.ScoreTeam1;
                bett.ScoreTeam2 = bettings.ScoreTeam2;
                
                
                _context.Add(bett);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bettings);
        }

        // GET: Bettings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bettings == null)
            {
                return NotFound();
            }

            var bettings = await _context.Bettings.FindAsync(id);
            if (bettings == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Id", bettings.GameId);
            ViewData["UserID"] = new SelectList(_context.AppUsers, "Id", "Id", bettings.UserID);
            return View(bettings);
        }

        // POST: Bettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserID,GameId,ScoreTeam1,ScoreTeam2")] Bettings bettings)
        {
            if (id != bettings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bettings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BettingsExists(bettings.Id))
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
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Id", bettings.GameId);
            ViewData["UserID"] = new SelectList(_context.AppUsers, "Id", "Id", bettings.UserID);
            return View(bettings);
        }

        // GET: Bettings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bettings == null)
            {
                return NotFound();
            }

            var bettings = await _context.Bettings
                .Include(b => b.Game)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bettings == null)
            {
                return NotFound();
            }

            return View(bettings);
        }

        // POST: Bettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bettings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bettings'  is null.");
            }
            var bettings = await _context.Bettings.FindAsync(id);
            if (bettings != null)
            {
                _context.Bettings.Remove(bettings);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BettingsExists(int id)
        {
            return _context.Bettings.Any(e => e.Id == id);
        }
    }
}
