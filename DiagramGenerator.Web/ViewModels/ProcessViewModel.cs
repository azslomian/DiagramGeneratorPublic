using DiagramGenerator.Web.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels
{
    public class ProcessViewModel : IUserViewModel
    {
        [Required]
        public Guid Id { get; set; }

        public int Lp { get; set; }

        [Required]
        public Guid DiagramId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Too short Name")]
        public string Name { get; set; }


        [Required]
        [MinLength(5, ErrorMessage = "Too short Purpose")]
        public string Purpose { get; set; }

        public string Description { get; set; }

        public IList<OperationViewModel> Operations { get; set; }

        public string UserEmail { get; set; }
    }
}
