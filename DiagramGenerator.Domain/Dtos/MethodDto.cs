using DiagramGenerator.Domain.Dtos.Abstract;
using System;

namespace DiagramGenerator.DataAccess.Model
{
    public class MethodDto : EntityDto
    {
        public Guid DiagramId { get; set; }
    }
}
