using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramGenerator.Domain.Services
{
    public class OutputManager : IOutputManager
    {
        private readonly IOutputRepository outputRepository;

        public OutputManager(IOutputRepository outputRepository)
        {
            this.outputRepository = outputRepository;
        }

        public Output GetById(Guid id)
        {
            return outputRepository.GetById(id).Result;
        }
        public IList<Output> GetAll(string email)
        {
            return outputRepository.GetAll(email).ToList();
        }

        public void Create(Output output)
        {
            var last = outputRepository.GetLast(output.UserEmail).Result;
            output.Lp = last == null ? 1 : ++last.Lp;
            outputRepository.Create(output).Wait();
        }

        public void Update(Guid id, Output output)
        {
            outputRepository.Update(id ,output).Wait();
        }

        public void Delete(Guid id)
        {
            outputRepository.Delete(id).Wait();
        }
    }
}
