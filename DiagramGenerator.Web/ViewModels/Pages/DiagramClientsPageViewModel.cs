using DiagramGenerator.Web.ViewModels.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels.Pages
{
    public class DiagramClientsPageViewModel
    {
        public Guid DiagramId { get; set; }

        public List<DiagramClientViewModel> Clients { get; set; }
    }
}
