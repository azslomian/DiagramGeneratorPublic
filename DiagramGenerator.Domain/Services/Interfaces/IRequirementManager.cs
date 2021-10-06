using DiagramGenerator.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Domain.Services.Interfaces
{
    public interface IRequirementManager
    {
        Requirement GetById(Guid id);

        IList<Requirement> GetAll(string email);

        void Create(Requirement requirement);

        void Update(Guid id, Requirement requirement);

        void Delete(Guid id);
    }
}