using DiagramGenerator.DataAccess.Abstract;
using DiagramGenerator.DataAccess.Model.ManyToMany;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagramGenerator.DataAccess.Model
{
    [Index(nameof(Id))]
    public class Supplier : UserEntity
    {
        //public ICollection<InputSupplier> Inputs { get; set; }
    }
}
