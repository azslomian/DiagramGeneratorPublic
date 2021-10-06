using DiagramGenerator.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Domain.Services.Interfaces
{
    public interface ICriterionManager
    {
        Criterion GetById(Guid id);

        IList<Criterion> GetAll(string email);

        void Create(Criterion criterion);

        void Update(Guid id, Criterion criterion);

        void Delete(Guid id);
    }
}