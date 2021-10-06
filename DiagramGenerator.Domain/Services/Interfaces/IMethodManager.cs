using DiagramGenerator.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Domain.Services.Interfaces
{
    public interface IMethodManager
    {
        Method GetById(Guid id);

        IList<Method> GetAll(string email);

        void Create(Method method);

        void Update(Guid id, Method method);

        void Delete(Guid id);
    }
}