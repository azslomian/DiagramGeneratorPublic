using DiagramGenerator.DataAccess.Model.Enum;
using DiagramGenerator.Domain.Dtos.Abstract;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.DataAccess.Model
{
    public class InputDto : QuantityEntityDto
    {
        public Guid DiagramId { get; set; }

        public InputType Type { get; set; }

        public IList<SupplierDto> Suppliers {get; set; }
    }
}
