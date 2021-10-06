using DiagramGenerator.DataAccess.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagramGenerator.DataAccess.Abstract
{
    public abstract class UserEntity : Entity
    {
        public string UserEmail { get; set; }

        [ForeignKey("Email")]
        public User User { get; set; }
    }
}
