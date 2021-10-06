using DiagramGenerator.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Domain.Services.Interfaces
{
    public interface ISupplierManager
    {
        Supplier GetById(Guid id);

        IList<Supplier> GetAll(string email);

        void Create(Supplier supplier);

        void Update(Guid id, Supplier supplier);

        void Delete(Guid id);
    }
}