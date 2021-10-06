using DiagramGenerator.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Domain.Repositories.Interfaces
{
    public interface IDiagramRepository : IUserGenericRepository<Diagram>
    {
        List<Diagram> GetDiagramsByUser(string username);

        Diagram GetDiagramById(Guid id);

        Diagram GetDiagramIncludeAll(Guid id);
    }
}