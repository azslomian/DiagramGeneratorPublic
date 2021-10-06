using System;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.Domain.Dtos.Abstract
{
    public abstract class EntityDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
