using DiagramGenerator.Web.ViewModels.Diagram;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Web.ViewModels.Pages
{
    public class DiagramCriteriumPageViewModel
    {
        public Guid DiagramId { get; set; }

        public List<DiagramCriterionViewModel> Criteria { get; set; }
    }
}
