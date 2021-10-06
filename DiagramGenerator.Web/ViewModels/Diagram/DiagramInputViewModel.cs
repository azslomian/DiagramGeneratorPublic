using DiagramGenerator.DataAccess.Model.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels.Diagram
{
    public class DiagramInputViewModel
    {
        [Required]
        public Guid Id { get; set; }

        public int Lp { get; set; }

        public string UserEmail { get; set; }

        [Required]
        public Guid DiagramId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Too short Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public int Quantity { get; set; }

        public InputType Type { get; set; }

        public bool IsSelected { get; set; }
    }
}
