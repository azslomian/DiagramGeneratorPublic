using DiagramGenerator.Web.ViewModels.Diagram;
using DiagramGenerator.Web.ViewModels.Helpers;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Web.ViewModels.Pages
{
    public class DiagramSummaryPageViewModel
    {
        public Guid DiagramId { get; set; }

        public int Lp { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string UserEmail { get; set; }

        public ProcessViewModel Process { get; set; }

        public List<DiagramInputViewModel> Inputs { get; set; }

        public List<DiagramOutputViewModel> Outputs { get; set; }

        public List<DiagramMethodViewModel> Methods { get; set; }

        public List<DiagramRequirementViewModel> Requirements { get; set; }

        public List<DiagramCriterionViewModel> Criteria { get; set; }

        public List<DiagramClientViewModel> Clients { get; set; }

        public List<DiagramSupplierViewModel> Suppliers { get; set; }

        public List<ValidationViewModel> Validation { get; set; }
    }
}
