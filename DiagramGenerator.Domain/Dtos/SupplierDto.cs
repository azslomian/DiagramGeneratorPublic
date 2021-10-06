using DiagramGenerator.Domain.Dtos.Abstract;
using System;

namespace DiagramGenerator.DataAccess.Model
{
    public class SupplierDto : EntityDto
    {
        public Guid InputId { get; set; }
    }
}
