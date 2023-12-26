using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Mundial.Models
{
    public class Game
    {
        public int Id { get; set; }
        [Display(Name = "Data i Godzina")]
        public DateTime DateTime { get; set; } = DateTime.Now;
        [Display(Name = "Kraje")]
        public ICollection<GameCountry>? gameCountries { get; set; }
        [Display(Name = "Wynik 1")]
        public int? ScoreCountry1 { get; set; } = 0;
        [Display(Name = "Wynik 2")]
        public int? ScoreCountry2 { get; set; } = 0;
        public bool IsEditable { get; set; } = true;
        public bool IsActive
        {
            get
            {
                if (DateTime > DateTime.Now)
                {
                    return false;
                }
                else return true;
            }
        }


    }

}
