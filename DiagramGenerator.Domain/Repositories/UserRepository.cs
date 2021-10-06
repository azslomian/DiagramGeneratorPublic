using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace DiagramGenerator.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DiagramGeneratorContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(DiagramGeneratorContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddUserAsync(User user, string password)
        {
            _context.Database.EnsureCreated();
            var existingUser = await _userManager.FindByEmailAsync(user.UserName);

            if(existingUser == null)
            {
                var result = await _userManager.CreateAsync(user, password);

                if(result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could no create new user");
                }
            }
        }
    }
}
