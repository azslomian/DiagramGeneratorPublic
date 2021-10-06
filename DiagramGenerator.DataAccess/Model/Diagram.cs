using DiagramGenerator.DataAccess.Abstract;
using DiagramGenerator.DataAccess.Model.ManyToMany;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagramGenerator.DataAccess.Model
{
    [Index(nameof(Id))]
    public class Diagram : UserEntity
    {
        public Process Process { get; set; }

        public ICollection<DiagramInput> Inputs {get; set; }

        public ICollection<DiagramOutput> Outputs { get; set; }

        public ICollection<DiagramMethod> Methods { get; set; }

        public ICollection<DiagramRequirement> Requirements { get; set; }

        public ICollection<DiagramCriterion> Criteria { get; set; }

        public ICollection<DiagramClient> Clients { get; set; }

        public ICollection<DiagramSupplier> Suppliers { get; set; }

        public void SetProcess(Process process)
        {
            Process = process;
        }
    }
}
