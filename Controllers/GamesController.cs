using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mundial.Data;
using Mundial.Models;
using Mundial.ViewModels;

namespace Mundial.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _context.Game.Include(c => c.gameCountries).ThenInclude(g => g.Country).ThenInclude(g => g.Grupa).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        public IActionResult Create()
        {
            List<Country> countries = _context.Countries.ToList();
            GameViewModel gameViewModel = new GameViewModel();
            gameViewModel.Countries = countries;
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var country in countries)
            {
                selectList.Add(new SelectListItem()
                {
                    Value = country.Id.ToString(),
                    Text = country.Name
                });
            }
            gameViewModel.CountriesSelected = selectList;

            return View(gameViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameViewModel gameVM)
        {
            if (ModelState.IsValid)
            {
                Game game = new Game();
                game.DateTime = gameVM.DateTime;
                game.ScoreCountry1 = gameVM.GoalsTeam1;
                game.ScoreCountry2 = gameVM.GoalsTeam2;
                _context.Add(game);
                await _context.SaveChangesAsync();
                GameCountry gameCountry = new GameCountry();
                GameCountry gameCountry2 = new GameCountry();
                gameCountry.GameId = game.Id;
                gameCountry.CountryId = gameVM.Country1Id;
                gameCountry2.GameId = game.Id;
                gameCountry2.CountryId = gameVM.Country2Id;
                _context.Add(gameCountry);
                await _context.SaveChangesAsync();
                _context.Add(gameCountry2);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }
        public async Task<IActionResult> Save(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }
            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateTime,ScoreCountry1,ScoreCountry2")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
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
            return View(game);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(int id, [Bind("Id,DateTime,ScoreCountry1,ScoreCountry2")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    game.IsEditable = false;
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                List<GameCountry> gamecountry = _context.GameCountry.Where(i => i.GameId == game.Id).ToList();
                List<Bettings> betts = _context.Bettings.Where(i => i.GameId == game.Id).ToList();
                int? countryId1 = gamecountry[0].CountryId;
                int? countryId2 = gamecountry[1].CountryId;
                Country team1 = _context.Countries.FirstOrDefault(i => i.Id == countryId1);
                Country team2 = _context.Countries.FirstOrDefault(i => i.Id == countryId2);
                foreach (var bet in betts)
                {
                    if (bet.ScoreTeam1 == game.ScoreCountry1 && bet.ScoreTeam2 == game.ScoreCountry2)
                    {
                        bet.BetPoints += 3;
                    }
                    else if (game.ScoreCountry1 == game.ScoreCountry2 && bet.ScoreTeam1 == bet.ScoreTeam2)
                    {
                        bet.BetPoints += 1;
                    }
                    else if (game.ScoreCountry1 > game.ScoreCountry2 && bet.ScoreTeam1 > bet.ScoreTeam2)
                    {
                        bet.BetPoints += 1;
                    }
                    else if (game.ScoreCountry1 < game.ScoreCountry2 && bet.ScoreTeam1 < bet.ScoreTeam2)
                    {
                        bet.BetPoints += 1;
                    }
                    _context.Update(bet);

                }
                if (game.ScoreCountry1 > game.ScoreCountry2)
                {
                    team1.Points += 3;
                    team1.GoalLost += game.ScoreCountry2;
                    team1.GoalScored += game.ScoreCountry1;
                    team2.GoalLost += game.ScoreCountry1;
                    team2.GoalScored += game.ScoreCountry2;
                    _context.Update(team1);
                    await _context.SaveChangesAsync();
                    _context.Update(team2);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                else if (game.ScoreCountry1 < game.ScoreCountry2)
                {
                    team2.Points += 3;
                    team2.GoalLost += game.ScoreCountry1;
                    team2.GoalScored += game.ScoreCountry2;
                    team1.GoalLost += game.ScoreCountry2;
                    team1.GoalScored += game.ScoreCountry1;
                    _context.Update(team1);
                    await _context.SaveChangesAsync();
                    _context.Update(team2);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    team2.Points += 1;
                    team1.Points += 1;
                    team2.GoalLost += game.ScoreCountry1;
                    team2.GoalScored += game.ScoreCountry2;
                    team1.GoalLost += game.ScoreCountry2;
                    team1.GoalScored += game.ScoreCountry1;
                    _context.Update(team1);
                    await _context.SaveChangesAsync();
                    _context.Update(team2);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }



            }
            return View(game);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Game == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Game'  is null.");
            }
            var game = await _context.Game.FindAsync(id);
            if (game != null)
            {
                var gameCountry = _context.GameCountry.Where(x => x.GameId == id).ToList();
                int? countryId1 = gameCountry[0].CountryId;
                int? countryId2 = gameCountry[1].CountryId;
                Country team1 = _context.Countries.FirstOrDefault(i => i.Id == countryId1);
                Country team2 = _context.Countries.FirstOrDefault(i => i.Id == countryId2);
                List<Bettings> betts = _context.Bettings.Where(i => i.GameId == game.Id).ToList();

                if (game.IsEditable == false)
                {
                    foreach (var bet in betts)
                    {
                        if (bet.ScoreTeam1 == game.ScoreCountry1 && bet.ScoreTeam2 == game.ScoreCountry2)
                        {
                            bet.BetPoints -= 3;
                        }
                        else if (game.ScoreCountry1 == game.ScoreCountry2 && bet.ScoreTeam1 == bet.ScoreTeam2)
                        {
                            bet.BetPoints -= 1;
                        }
                        else if (game.ScoreCountry1 > game.ScoreCountry2 && bet.ScoreTeam1 > bet.ScoreTeam2)
                        {
                            bet.BetPoints -= 1;
                        }
                        else if (game.ScoreCountry1 < game.ScoreCountry2 && bet.ScoreTeam1 < bet.ScoreTeam2)
                        {
                            bet.BetPoints -= 1;
                        }
                        _context.Remove(bet);

                    }

                    if (game.ScoreCountry1 > game.ScoreCountry2)
                    {
                        team1.Points -= 3;
                        team1.GoalLost -= game.ScoreCountry2;
                        team1.GoalScored -= game.ScoreCountry1;
                        team2.GoalLost -= game.ScoreCountry1;
                        team2.GoalScored -= game.ScoreCountry2;
                        _context.Update(team1);
                        _context.Update(team2);

                    }
                    else if (game.ScoreCountry1 < game.ScoreCountry2)
                    {
                        team2.Points -= 3;
                        team2.GoalLost -= game.ScoreCountry1;
                        team2.GoalScored -= game.ScoreCountry2;
                        team1.GoalLost -= game.ScoreCountry2;
                        team1.GoalScored -= game.ScoreCountry1;
                        _context.Update(team1);
                        _context.Update(team2);

                    }
                    else
                    {
                        team2.Points -= 1;
                        team1.Points -= 1;
                        team2.GoalLost -= game.ScoreCountry1;
                        team2.GoalScored -= game.ScoreCountry2;
                        team1.GoalLost -= game.ScoreCountry2;
                        team1.GoalScored -= game.ScoreCountry1;
                        _context.Update(team1);
                        _context.Update(team2);

                    }
                }
                foreach (var country in gameCountry)
                {
                    _context.GameCountry.Remove(country);
                }
                _context.Game.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.Id == id);
        }
    }
}
