using DiagramGenerator.Domain.Dtos.Abstract;
using System;

namespace DiagramGenerator.DataAccess.Model
{
    public class CriterionDto : EntityDto
    {
        public Guid DiagramId { get; set; }
    }
}
