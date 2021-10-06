using DiagramGenerator.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagramGenerator.DataAccess.Model.ManyToMany
{
    public class DiagramOutput : QuantityEntity
    {
        public Guid DiagramId { get; set; }

        [ForeignKey("DiagramId")]
        public Diagram Diagram { get; set; }

        public Guid OutputId { get; set; }

        [ForeignKey("OutputId")]
        public Output Output { get; set; }
    }
}
