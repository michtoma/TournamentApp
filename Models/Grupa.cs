using System.ComponentModel.DataAnnotations;

namespace Mundial.Models
{
    public class Grupa
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Country>? Country { get; set; }
    }
}
