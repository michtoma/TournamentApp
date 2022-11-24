using Microsoft.AspNetCore.Mvc.Rendering;
using Mundial.Models;

namespace Mundial.ViewModels
{
    public class GameViewModel
    {
        public List<Country>? Countries { get; set; }
        public IEnumerable<SelectListItem>? CountriesSelected { get; set; }
        public Game? Game { get; set; }
        public int? GameId { get; set; }
        public DateTime DateTime { get; set; }

        public int? Country1Id { get; set; }
        public int? Country2Id { get; set; }
        public int? GoalsTeam1 { get; set; } = 0;
        public int? GoalsTeam2 { get; set; } = 0;

    }
}
