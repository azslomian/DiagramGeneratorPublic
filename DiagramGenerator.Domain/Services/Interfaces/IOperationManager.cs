using DiagramGenerator.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Domain.Services.Interfaces
{
    public interface IOperationManager
    {
        Operation GetById(Guid id);

        IList<Operation> GetAll();

        void Create(Operation input);

        void Update(Guid id, Operation input);

        void Delete(Guid id);

        Operation GetByIdEager(Guid id);
    }
}