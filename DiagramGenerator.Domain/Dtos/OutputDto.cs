using DiagramGenerator.DataAccess.Model.Enum;
using DiagramGenerator.Domain.Dtos.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.DataAccess.Model
{
    public class OutputDto : QuantityEntityDto
    {
        public Guid DiagramId { get; set; }

        [Required]
        public OutputType Type { get; set; }
    }
}
