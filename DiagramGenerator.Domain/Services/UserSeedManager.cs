using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using DiagramGenerator.Domain.Services.Interfaces;
using System.Threading.Tasks;

namespace DiagramGenerator.Domain.Services
{
    public class UserSeedManager : IUserSeedManager
    {
        private readonly IUserRepository userRepository;

        public UserSeedManager(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task AddUser(User user, string password)
        {
            await userRepository.AddUserAsync(user, password);
        }
    }
}
