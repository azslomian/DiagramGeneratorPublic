using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.DataAccess.Model.ManyToMany;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramGenerator.Domain.Services
{
    public class DiagramManager : IDiagramManager
    {
        private readonly IDiagramRepository diagramRepository;
        private readonly IProcessRepository processRepository;

        public DiagramManager(IDiagramRepository diagramRepository, IProcessRepository processRepository)
        {
            this.diagramRepository = diagramRepository;
            this.processRepository = processRepository;
        }

        public List<Diagram> GetDiagramsByUser(string username)
        {
            return diagramRepository.GetDiagramsByUser(username);
        }

        public Diagram GetDiagramIncludeAll(Guid id)
        {
            var diagram = diagramRepository.GetDiagramIncludeAll(id);
            return diagram;
        }

        public Diagram GetDiagramById(Guid id)
        {
            return diagramRepository.GetById(id).Result;
        }

        public Diagram GetDiagramByIdEager(Guid id)
        {
            return diagramRepository.GetByIdEager(id);
        }

        public Diagram GetById(Guid id)
        {
            return diagramRepository.GetById(id).Result;
        }
        public IList<Diagram> GetAll(string email)
        {
            return diagramRepository.GetAll(email).ToList();
        }

        public void Create(Diagram diagram)
        {
            var last = diagramRepository.GetLast(diagram.UserEmail).Result;
            diagram.Lp = last == null ? 1 : ++last.Lp;
            diagramRepository.Create(diagram).Wait();
        }

        public void AddProcess(Process process)
        {
            var diagram = diagramRepository.GetDiagramById(process.DiagramId);
            var last = processRepository.GetLast().Result;
            process.Lp = last == null ? 1 : ++last.Lp;
            diagram.SetProcess(process);
            diagramRepository.SaveChanges();
            diagramRepository.ChangeTrackerClear();
        }

        public void Update(Guid id, Diagram diagram)
        {
            diagramRepository.Update(id, diagram).Wait();
        }

        public void Delete(Guid id)
        {
            diagramRepository.Delete(id).Wait();
        }

        public Diagram GetByIdEager(Guid id)
        {
            return diagramRepository.GetByIdEager(id);
        }

        public void UpdateInputs(Guid diagramId, List<DiagramInput> diagramInputs)
        {
            var diagram = GetByIdEager(diagramId);
            diagram.Inputs = diagramInputs;
            diagramRepository.SaveChanges();
        }

        public void UpdateOutputs(Guid diagramId, List<DiagramOutput> diagramOutputs)
        {
            var diagram = GetByIdEager(diagramId);
            diagram.Outputs = diagramOutputs;
            diagramRepository.SaveChanges();
        }

        public void UpdateMethods(Guid diagramId, List<DiagramMethod> diagramMethods)
        {
            var diagram = GetByIdEager(diagramId);
            diagram.Methods = diagramMethods;
            diagramRepository.SaveChanges();
        }

        public void UpdateRequirements(Guid diagramId, List<DiagramRequirement> diagramRequirements)
        {
            var diagram = GetByIdEager(diagramId);
            diagram.Requirements = diagramRequirements;
            diagramRepository.SaveChanges();
        }

        public void UpdateCriteria(Guid diagramId, List<DiagramCriterion> diagramCriteria)
        {
            var diagram = GetByIdEager(diagramId);
            diagram.Criteria = diagramCriteria;
            diagramRepository.SaveChanges();
        }

        public void UpdateClients(Guid diagramId, List<DiagramClient> diagramClients)
        {
            var diagram = GetByIdEager(diagramId);
            diagram.Clients = diagramClients;
            diagramRepository.SaveChanges();
        }

        public void UpdateSuppliers(Guid diagramId, List<DiagramSupplier> diagramSupplier)
        {
            var diagram = GetByIdEager(diagramId);
            diagram.Suppliers = diagramSupplier;
            diagramRepository.SaveChanges();
        }
    }
}
