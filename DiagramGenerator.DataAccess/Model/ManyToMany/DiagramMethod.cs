using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagramGenerator.DataAccess.Model.ManyToMany
{
    public class DiagramMethod
    {
        public Guid DiagramId { get; set; }

        [ForeignKey("DiagramId")]
        public Diagram Diagram { get; set; }

        public Guid MethodId { get; set; }

        [ForeignKey("MethodId")]
        public Method Method { get; set; }
    }
}
