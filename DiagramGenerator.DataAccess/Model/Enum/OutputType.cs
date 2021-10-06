using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiagramGenerator.DataAccess.Model.Enum
{
    public enum OutputType
    {
        [Display(Name = "Product")]
        Product = 1,

        [Display(Name = "Document")]
        Document = 2,

        [Display(Name = "Output")]
        Other = 3
    }
}
