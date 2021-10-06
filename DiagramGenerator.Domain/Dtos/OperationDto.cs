using DiagramGenerator.Domain.Dtos.Abstract;
using System;

namespace DiagramGenerator.DataAccess.Model
{
    public class OperationDto : EntityDto
    {
        public Guid DiagramId { get; set; }

        public Guid ProcessId { get; set; }
    }
}
