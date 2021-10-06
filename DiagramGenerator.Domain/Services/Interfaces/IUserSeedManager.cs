using DiagramGenerator.DataAccess.Model;
using System;
using System.Threading.Tasks;

namespace DiagramGenerator.Domain.Services.Interfaces
{
    public interface IUserSeedManager
    {
        Task AddUser(User user, string password);
    }
}