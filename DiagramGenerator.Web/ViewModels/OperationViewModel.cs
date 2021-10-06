using DiagramGenerator.DataAccess.Model.Enum;
using DiagramGenerator.Web.ViewModels.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels
{
    public class OperationViewModel : IUserViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid ProcessId { get; set; }

        [Required]
        public Guid DiagramId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Too short Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int TimeInMinutes { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Employees { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Lp { get; set; }

        public OperationType Type { get; set; }

        public string UserEmail { get; set; }
    }
}
