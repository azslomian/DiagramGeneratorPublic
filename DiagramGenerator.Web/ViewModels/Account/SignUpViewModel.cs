using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels.Account
{
    public class SignUpViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must meet requirements (one number, one capital letter, one number, one special character and at least 8 characters)")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
