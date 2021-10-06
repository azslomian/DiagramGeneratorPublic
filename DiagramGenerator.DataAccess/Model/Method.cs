using DiagramGenerator.DataAccess.Abstract;
using DiagramGenerator.DataAccess.Model.ManyToMany;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagramGenerator.DataAccess.Model
{
    [Index(nameof(Id))]
    public class Method : UserEntity
    {
        //public ICollection<DiagramMethod> Diagrams { get; set; }
    }
}
