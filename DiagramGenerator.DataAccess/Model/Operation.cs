using DiagramGenerator.DataAccess.Abstract;
using DiagramGenerator.DataAccess.Model.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagramGenerator.DataAccess.Model
{
    [Index(nameof(Id))]
    public class Operation : Entity
    {
        public Guid ProcessId { get; set; }

        [ForeignKey("ProcessId")]
        public Process Process { get; set; }

        public int TimeInMinutes { get; set; }

        public int Employees { get; set; }

        public OperationType Type { get; set; }
    }
}
