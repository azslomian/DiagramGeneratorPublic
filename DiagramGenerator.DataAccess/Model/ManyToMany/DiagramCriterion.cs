using DiagramGenerator.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagramGenerator.DataAccess.Model.ManyToMany
{
    public class DiagramCriterion 
    {
        public Guid DiagramId { get; set; }

        [ForeignKey("DiagramId")]
        public Diagram Diagram { get; set; }

        public Guid CriterionId { get; set; }

        [ForeignKey("CriterionId")]
        public Criterion Criterion { get; set; }
    }
}
