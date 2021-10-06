using DiagramGenerator.Web.ViewModels.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels.Pages
{
    public class DiagramInputPageViewModel
    {
        public Guid DiagramId { get; set; }

        public List<DiagramInputViewModel> Inputs { get; set; }
    }
}
