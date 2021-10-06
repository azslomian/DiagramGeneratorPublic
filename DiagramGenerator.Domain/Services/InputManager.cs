using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramGenerator.Domain.Services
{
    public class InputManager : IInputManager
    {
        private readonly IInputRepository inputRepository;

        public InputManager(IInputRepository inputRepository)
        {
            this.inputRepository = inputRepository;
        }

        public Input GetById(Guid id)
        {
            return inputRepository.GetById(id).Result;
        }
        public IList<Input> GetAll(string email)
        {
            return inputRepository.GetAll(email).ToList();
        }

        public void Create(Input input)
        {
            var last = inputRepository.GetLast(input.UserEmail).Result;
            input.Lp = last == null ? 1 : ++last.Lp;
            inputRepository.Create(input).Wait();
        }

        public void Update(Guid id, Input input)
        {
            inputRepository.Update(id ,input).Wait();
        }

        public void Delete(Guid id)
        {
            inputRepository.Delete(id).Wait();
        }
    }
}
