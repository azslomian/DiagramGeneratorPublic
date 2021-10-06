using DiagramGenerator.DataAccess.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Web.ViewModels.Pages
{
    public class InputPageViewModel
    {
        public IList<InputViewModel> Inputs { get; set; }

        public IList<RequirementViewModel> Requirements { get; set; }

        public IList<MethodViewModel> Methods { get; set; }
    }
}
