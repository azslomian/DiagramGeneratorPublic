using AutoMapper;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramGenerator.Domain.Services
{
    public class OperationManager : IOperationManager
    {
        private readonly IOperationRepository operationRepository;
        private readonly IProcessRepository processRepository;
        private readonly IMapper mapper;

        public OperationManager(
            IOperationRepository operationRepository, 
            IProcessRepository processRepository,
            IMapper mapper)
        {
            this.operationRepository = operationRepository;
            this.processRepository = processRepository;
            this.mapper = mapper;
        }

        public Operation GetById(Guid id)
        {
            var operation = operationRepository.GetById(id).Result;
            return operation;
        }

        public Operation GetByIdEager(Guid id)
        {
            var operation = operationRepository.GetByIdEager(id);
            return operation;
        }

        public IList<Operation> GetAll()
        {
            return operationRepository.GetAll().ToList();
        }

        public void Create(Operation operation)
        {
            operationRepository.Create(operation).Wait();
        }

        public void Update(Guid id, Operation operation)
        {
            var process = processRepository.GetByIdEager(operation.ProcessId);
            var existingOperation = process.Operations.First(x => x.Id == id);
            existingOperation.Name = operation.Name;
            existingOperation.Description = operation.Description;
            existingOperation.Employees = operation.Employees;
            existingOperation.Type = operation.Type;
            existingOperation.TimeInMinutes = operation.TimeInMinutes;
            existingOperation.Lp = operation.Lp;

            operationRepository.SaveChanges();

            //var newOperation = new Operation
            //{
            //    Id = operation.Id,
            //    Description = operation.Description,
            //    Name = operation.Name, 
            //    Employees = operation.Employees,
            //    ProcessId = operation.ProcessId,
            //    Type = operation.Type,
            //    TimeInMinutes = operation.TimeInMinutes
            //};

            //dbContext.Entry(policyMapping).Property("PolicyAId").IsModified = true;
            //dbContext.Entry(policyMapping).Property("PolicyBId").IsModified = true;
            //dbContext.Entry(policyMapping).Property("BankId").IsModified = true;
            //dbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            operationRepository.Delete(id).Wait();
        }
    }
}
