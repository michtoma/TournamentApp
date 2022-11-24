using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Mundial.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Kraj")]
        public string Name { get; set; }
        public Grupa? Grupa { get; set; }
        public int GrupaId { get; set; }
        [Display(Name = "Punkty")]
        public int? Points { get; set; } = 0;
        [Display(Name = "Bramki zdobyte")]
        public int? GoalScored { get; set; } = 0;
        [Display(Name = "Bramki stracone")]
        public int? GoalLost { get; set; } = 0;
        [Display(Name = "Różnica bramek")]
        public int? GoalDif
        {
            get
            {
                return GoalScored - GoalLost;
            }
        }


    }

}
