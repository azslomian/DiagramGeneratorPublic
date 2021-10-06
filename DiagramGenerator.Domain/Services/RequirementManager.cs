using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramGenerator.Domain.Services
{
    public class RequirementManager : IRequirementManager
    {
        private readonly IRequirementRepository requirementRepository;

        public RequirementManager(IRequirementRepository requirementRepository)
        {
            this.requirementRepository = requirementRepository;
        }

        public Requirement GetById(Guid id)
        {
            return requirementRepository.GetById(id).Result;
        }
        public IList<Requirement> GetAll(string email)
        {
            return requirementRepository.GetAll(email).ToList();
        }

        public void Create(Requirement requirement)
        {
            var last = requirementRepository.GetLast(requirement.UserEmail).Result;
            requirement.Lp = last == null ? 1 : ++last.Lp;
            requirementRepository.Create(requirement).Wait();
        }

        public void Update(Guid id, Requirement requirement)
        {
            requirementRepository.Update(id , requirement).Wait();
        }

        public void Delete(Guid id)
        {
            requirementRepository.Delete(id).Wait();
        }
    }
}
