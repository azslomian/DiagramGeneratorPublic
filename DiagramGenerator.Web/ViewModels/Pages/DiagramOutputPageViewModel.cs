using DiagramGenerator.Web.ViewModels.Diagram;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Web.ViewModels.Pages
{
    public class DiagramOutputPageViewModel
    {
        public Guid DiagramId { get; set; }

        public List<DiagramOutputViewModel> Outputs { get; set; }
    }
}
