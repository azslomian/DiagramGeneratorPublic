using DiagramGenerator.DataAccess.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagramGenerator.DataAccess.Interfaces
{
    public interface IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Lp { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
