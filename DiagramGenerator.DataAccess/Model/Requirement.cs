using DiagramGenerator.DataAccess.Abstract;
using DiagramGenerator.DataAccess.Model.ManyToMany;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.DataAccess.Model
{
    [Index(nameof(Id))]
    public class Requirement : UserEntity
    {
        //public ICollection<DiagramRequirement> Diagrams { get; set; }
    }
}
