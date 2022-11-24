using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mundial.Models;

namespace Mundial.ViewModels
{
    public class BettingsViewModel
    {
        public string? UserId { get; set; }
        public int GameId { get; set; }
        public int ScoreTeam1 { get; set; } = 0;
        public int ScoreTeam2 { get; set; } = 0;
        public List<Game> Games { get; set; } = new List<Game>();
        public IEnumerable<SelectListItem>? GamesSelected { get; set; }
        public List<Bettings> Betts { get; set; } = new List<Bettings>();
        public IEnumerable<SelectListItem>? BettsSelected { get; set; }
    }
}
