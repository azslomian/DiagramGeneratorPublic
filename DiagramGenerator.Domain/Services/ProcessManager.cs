using AutoMapper;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramGenerator.Domain.Services
{
    public class ProcessManager : IProcessManager
    {
        private readonly IProcessRepository processRepository;

        public ProcessManager(IProcessRepository processRepository, IMapper mapper)
        {
            this.processRepository = processRepository;
        }

        public Process GetById(Guid id)
        {
            return processRepository.GetById(id).Result;
        }

        public Process GetProcessByDiagramId(Guid diagramId)
        {
            return processRepository.GetProcessByDiagramId(diagramId);
        }

        public IList<Process> GetAll()
        {
            return processRepository.GetAll().ToList();
        }

        public void Create(Process process)
        {
            var last = processRepository.GetLast().Result;
            process.Lp = last == null ? 1 : ++last.Lp;
            processRepository.Create(process).Wait();
        }

        public void Update(Guid id, Process process)
        {
            processRepository.Update(id , process).Wait();
        }

        public void Delete(Guid id)
        {
            processRepository.Delete(id).Wait();
            processRepository.ChangeTrackerClear();
        }

        public void AddOperation(Operation operation)
        {
            var process = processRepository.GetProcessById(operation.ProcessId);
            process.AddOperation(operation);
            processRepository.SaveChangesAsync();
        }

        public void RemoveOperation(Operation operation)
        {
            var process = processRepository.GetProcessById(operation.ProcessId);
            process.RemoveOperation(operation);
            processRepository.SaveChanges();
        }
    }
}
