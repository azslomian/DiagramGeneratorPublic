using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramGenerator.Domain.Services
{
    public class MethodManager : IMethodManager
    {
        private readonly IMethodRepository methodRepository;

        public MethodManager(IMethodRepository methodRepository)
        {
            this.methodRepository = methodRepository;
        }

        public Method GetById(Guid id)
        {
            return methodRepository.GetById(id).Result;
        }
        public IList<Method> GetAll(string email)
        {
            return methodRepository.GetAll(email).ToList();
        }

        public void Create(Method method)
        {
            var last = methodRepository.GetLast(method.UserEmail).Result;
            method.Lp = last == null ? 1 : ++last.Lp;
            methodRepository.Create(method).Wait();
        }

        public void Update(Guid id, Method method)
        {
            methodRepository.Update(id , method).Wait();
        }

        public void Delete(Guid id)
        {
            methodRepository.Delete(id).Wait();
        }
    }
}
