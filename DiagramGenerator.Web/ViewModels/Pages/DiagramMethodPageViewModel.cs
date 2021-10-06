using DiagramGenerator.Web.ViewModels.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels.Pages
{
    public class DiagramMethodPageViewModel
    {
        public Guid DiagramId { get; set; }

        public List<DiagramMethodViewModel> Methods { get; set; }
    }
}
