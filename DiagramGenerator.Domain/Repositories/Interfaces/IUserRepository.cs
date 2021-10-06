
using DiagramGenerator.DataAccess.Model;
using System.Threading.Tasks;

namespace DiagramGenerator.Domain.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user, string password);
    }
}