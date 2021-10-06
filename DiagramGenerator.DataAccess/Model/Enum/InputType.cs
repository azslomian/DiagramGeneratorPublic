using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiagramGenerator.DataAccess.Model.Enum
{
    public enum InputType
    {
        [Display(Name = "Human Resource")]
        HumanResource = 1,

        [Display(Name = "Material")]
        Material = 2,

        [Display(Name = "Document")]
        Document = 3,

        [Display(Name = "Equipment")]
        Equipment = 4,

        [Display(Name = "Input")]
        Other = 5
    }
}
