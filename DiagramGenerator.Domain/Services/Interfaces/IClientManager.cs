using DiagramGenerator.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace DiagramGenerator.Domain.Services.Interfaces
{
    public interface IClientManager
    {
        Client GetById(Guid id);

        IList<Client> GetAll(string email);

        void Create(Client client);

        void Update(Guid id, Client client);

        void Delete(Guid id);
    }
}