using DiagramGenerator.Web.ViewModels.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels.Pages
{
    public class DiagramSuppliersPageViewModel
    {
        public Guid DiagramId { get; set; }

        public List<DiagramSupplierViewModel> Suppliers { get; set; }
    }
}
