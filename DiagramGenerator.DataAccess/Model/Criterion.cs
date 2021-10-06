using DiagramGenerator.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System;

namespace DiagramGenerator.DataAccess.Model
{
    [Index(nameof(Id))]
    public class Criterion : UserEntity
    {
        //[JsonIgnore]
        //public ICollection<DiagramCriterion> Diagrams { get; set; }
    }
}
