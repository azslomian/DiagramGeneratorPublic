using DiagramGenerator.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Domain.Services.Interfaces
{
    public interface IProcessManager
    {
        Process GetById(Guid id);

        IList<Process> GetAll();

        void Create(Process input);

        void Update(Guid id, Process input);

        void Delete(Guid id);

        Process GetProcessByDiagramId(Guid diagramId);

        void AddOperation(Operation operation);

        void RemoveOperation(Operation operation);
    }
}