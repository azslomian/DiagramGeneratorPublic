using System;
using System.Collections.Generic;

namespace DiagramGenerator.Web.ViewModels.Pages
{
    public class ProcessPageViewModel
    {
        public Guid DiagramId { get; set; }

        public ProcessViewModel Process { get; set; }
    }
}
