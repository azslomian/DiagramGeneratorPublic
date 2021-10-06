using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramGenerator.Domain.Services
{
    public class CriterionManager : ICriterionManager
    {
        private readonly ICriterionRepository criterionRepository;

        public CriterionManager(ICriterionRepository criterionRepository)
        {
            this.criterionRepository = criterionRepository;
        }

        public Criterion GetById(Guid id)
        {
            return criterionRepository.GetById(id).Result;
        }
        public IList<Criterion> GetAll(string email)
        {
            return criterionRepository.GetAll(email).ToList();
        }

        public void Create(Criterion criterion)
        {
            var last = criterionRepository.GetLast(criterion.UserEmail).Result;
            criterion.Lp = last == null ? 1 : ++last.Lp;
            criterionRepository.Create(criterion).Wait();
        }

        public void Update(Guid id, Criterion criterion)
        {
            criterionRepository.Update(id , criterion).Wait();
        }

        public void Delete(Guid id)
        {
            criterionRepository.Delete(id).Wait();
        }
    }
}
