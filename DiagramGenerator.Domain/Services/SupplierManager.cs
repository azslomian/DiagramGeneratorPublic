using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramGenerator.Domain.Services
{
    public class SupplierManager : ISupplierManager
    {
        private readonly ISupplierRepository supplierRepository;

        public SupplierManager(ISupplierRepository supplierRepository)
        {
            this.supplierRepository = supplierRepository;
        }

        public Supplier GetById(Guid id)
        {
            return supplierRepository.GetById(id).Result;
        }
        public IList<Supplier> GetAll(string email)
        {
            return supplierRepository.GetAll(email).ToList();
        }

        public void Create(Supplier supplier)
        {
            var last = supplierRepository.GetLast(supplier.UserEmail).Result;
            supplier.Lp = last == null ? 1 : ++last.Lp;
            supplierRepository.Create(supplier).Wait();
        }

        public void Update(Guid id, Supplier supplier)
        {
            supplierRepository.Update(id , supplier).Wait();
        }

        public void Delete(Guid id)
        {
            supplierRepository.Delete(id).Wait();
        }
    }
}
