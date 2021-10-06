using DiagramGenerator.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagramGenerator.DataAccess.Model.ManyToMany
{
    public class UserDiagram
    {
        public Guid DiagramId { get; set; }

        [ForeignKey("DiagramId")]
        public Diagram Diagram { get; set; }

        public string UserMail { get; set; }

        [ForeignKey("UserMail")]
        public User User { get; set; }
    }
}
