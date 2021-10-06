using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramGenerator.Domain.Services
{
    public class ClientManager : IClientManager
    {
        private readonly IClientRepository clientRepository;

        public ClientManager(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public Client GetById(Guid id)
        {
            return clientRepository.GetById(id).Result;
        }
        public IList<Client> GetAll(string email)
        {
            return clientRepository.GetAll(email).ToList();
        }

        public void Create(Client client)
        {
            var last = clientRepository.GetLast(client.UserEmail).Result;
            client.Lp = last == null ? 1 : ++last.Lp;
            clientRepository.Create(client).Wait();
        }

        public void Update(Guid id, Client client)
        {
            clientRepository.Update(id , client).Wait();
        }

        public void Delete(Guid id)
        {
            clientRepository.Delete(id).Wait();
        }
    }
}
