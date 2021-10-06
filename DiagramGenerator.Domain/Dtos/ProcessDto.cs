using DiagramGenerator.Domain.Dtos.Abstract;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.DataAccess.Model
{
    public class ProcessDto : EntityDto
    {
        public Guid DiagramId { get; set; }

        public ICollection<OperationDto> Operations {get; set; }
    }
}
