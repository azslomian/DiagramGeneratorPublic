using DiagramGenerator.Domain.Dtos.Abstract;
using System;

namespace DiagramGenerator.DataAccess.Model
{
    public class RequirementDto : EntityDto
    {
        public Guid DiagramId { get; set; }
    }
}
