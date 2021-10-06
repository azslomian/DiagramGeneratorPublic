using DiagramGenerator.DataAccess.Abstract;
using DiagramGenerator.DataAccess.Model.Enum;
using DiagramGenerator.DataAccess.Model.ManyToMany;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagramGenerator.DataAccess.Model
{
    [Index(nameof(Id))]
    public class Output : UserEntity
    {
        [Required]
        public OutputType Type { get; set; }

        //public ICollection<DiagramOutput> Diagrams { get; set; }
    }
}
