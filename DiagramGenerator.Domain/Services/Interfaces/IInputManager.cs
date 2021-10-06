using DiagramGenerator.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Domain.Services.Interfaces
{
    public interface IInputManager
    {
        Input GetById(Guid id);

        IList<Input> GetAll(string email);

        void Create(Input input);

        void Update(Guid id, Input input);

        void Delete(Guid id);
    }
}