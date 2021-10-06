using System.Collections.Generic;

namespace DiagramGenerator.Web.ViewModels.Pages
{
    public class OutputPageViewModel
    {
        public IList<OutputViewModel> Outputs { get; set; }

        public IList<CriterionViewModel> Criteria { get; set; }
    }
}
