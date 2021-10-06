using DiagramGenerator.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagramGenerator.DataAccess.Model.ManyToMany
{
    public class DiagramClient
    {
        public Guid DiagramId { get; set; }

        [ForeignKey("DiagramId")]
        public Diagram Diagram { get; set; }

        public Guid ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
