using DiagramGenerator.DataAccess.Model.Enum;
using DiagramGenerator.Web.ViewModels.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels
{
    public class OutputViewModel : IUserViewModel
    {
        [Required]
        public Guid Id { get; set; }

        public int Lp { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Too short Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public OutputType Type { get; set; }

        public string UserEmail { get; set; }
    }
}
