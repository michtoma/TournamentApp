using System.ComponentModel.DataAnnotations;

namespace Mundial.Models
{
    public class GameCountry
    {
        [Key]
        public int Id { get; set; }
        public int? CountryId { get; set; }
        public int? GameId { get; set; }
        public Country? Country { get; set; }
        public Game? Game { get; set; }
    }
}
