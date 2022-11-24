using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mundial.Models
{
    public class AppUsers : IdentityUser
    {
        public string? FullName { get; set; }
        public string? NickName { get; set; }
        [NotMapped]
        public override bool EmailConfirmed { get; set; }
        public int Points { get; set; } = 0;

    }
}
