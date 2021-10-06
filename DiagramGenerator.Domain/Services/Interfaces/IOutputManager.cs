using DiagramGenerator.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Domain.Services.Interfaces
{
    public interface IOutputManager
    {
        Output GetById(Guid id);

        IList<Output> GetAll(string email);

        void Create(Output output);

        void Update(Guid id, Output output);

        void Delete(Guid id);
    }
}