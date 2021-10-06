
using DiagramGenerator.DataAccess.Model;
using System;

namespace DiagramGenerator.Domain.Repositories.Interfaces
{
    public interface IProcessRepository : IGenericRepository<Process>
    {
        Process GetProcessByDiagramId(Guid diagramId);

        Process GetProcessById(Guid id);
    }
}