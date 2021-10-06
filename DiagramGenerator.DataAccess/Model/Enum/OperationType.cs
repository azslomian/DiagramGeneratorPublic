using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiagramGenerator.DataAccess.Model.Enum
{
    public enum OperationType
    {
        [Display(Name = "Manufacturing")]
        Manufacturing = 1,

        [Display(Name = "Control")]
        Control = 2,

        [Display(Name = "Transport")]
        Transport = 3,

        [Display(Name = "Storage")]
        Storage = 4
    }
}
