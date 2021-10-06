using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels.Account
{
    public class ContactViewModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Too short Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
