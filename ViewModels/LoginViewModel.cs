using System.ComponentModel.DataAnnotations;

namespace Mundial.ViewModels
{
    public class LoginViewModel
    {
        
      
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}

