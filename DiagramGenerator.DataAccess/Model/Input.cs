using DiagramGenerator.DataAccess.Abstract;
using DiagramGenerator.DataAccess.Model.Enum;
using DiagramGenerator.DataAccess.Model.ManyToMany;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiagramGenerator.DataAccess.Model
{
    [Index(nameof(Id))]
    public class Input : UserEntity
    {
        [Required]
        public InputType Type { get; set; }

        //public ICollection<DiagramInput> Diagrams { get; set; }
    }
}
