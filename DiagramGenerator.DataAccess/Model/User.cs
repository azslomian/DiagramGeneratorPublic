using DiagramGenerator.DataAccess.Model.ManyToMany;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DiagramGenerator.DataAccess.Model
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<UserDiagram> Diagrams { get; set; }
    }
}
