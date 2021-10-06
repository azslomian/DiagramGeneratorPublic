using System;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels.Interfaces
{
    public interface IUserViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Too short Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string UserEmail { get; set; }
    }
}
