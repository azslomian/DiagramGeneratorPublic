using DiagramGenerator.DataAccess.Model.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels.Diagram
{
    public class DiagramRequirementViewModel
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

        public bool IsSelected { get; set; }
    }
}
