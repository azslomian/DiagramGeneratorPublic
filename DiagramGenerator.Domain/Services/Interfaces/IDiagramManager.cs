using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.DataAccess.Model.ManyToMany;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Domain.Services.Interfaces
{
    public interface IDiagramManager
    {
        Diagram GetDiagramById(Guid id);

        List<Diagram> GetDiagramsByUser(string username);

        Diagram GetById(Guid id);

        Diagram GetByIdEager(Guid id);

        Diagram GetDiagramIncludeAll(Guid id);

        IList<Diagram> GetAll(string email);

        void Create(Diagram input);

        void Update(Guid id, Diagram input);

        void Delete(Guid id);

        void AddProcess(Process process);

        void UpdateInputs(Guid diagramId, List<DiagramInput> diagramInputs);

        void UpdateMethods(Guid diagramId, List<DiagramMethod> diagramMethods);

        void UpdateRequirements(Guid diagramId, List<DiagramRequirement> diagramRequirements);

        void UpdateOutputs(Guid diagramId, List<DiagramOutput> diagramOutputs);

        void UpdateCriteria(Guid diagramId, List<DiagramCriterion> diagramCriteria);

        void UpdateClients(Guid diagramId, List<DiagramClient> diagramClients);

        void UpdateSuppliers(Guid diagramId, List<DiagramSupplier> diagramSuppliers);
    }
}