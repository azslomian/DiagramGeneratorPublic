using DiagramGenerator.DataAccess.Abstract;
using DiagramGenerator.DataAccess.Model.ManyToMany;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.DataAccess.Model
{
    [Index(nameof(Id))]
    public class Client : UserEntity
    {
        //public ICollection<DiagramClient> Diagrams { get; set; }
    }
}
