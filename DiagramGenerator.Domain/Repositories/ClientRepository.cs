using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;

namespace DiagramGenerator.Domain.Repositories
{
    public class ClientRepository : UserGenericRepository<Client>, IClientRepository
    {
        private readonly DiagramGeneratorContext _context;

        public ClientRepository(DiagramGeneratorContext context) : base(context)
        {
            _context = context;
        }
    }
}
