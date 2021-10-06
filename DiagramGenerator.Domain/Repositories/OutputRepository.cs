using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;

namespace DiagramGenerator.Domain.Repositories
{
    public class OutputRepository : UserGenericRepository<Output>, IOutputRepository
    {
        private readonly DiagramGeneratorContext _context;

        public OutputRepository(DiagramGeneratorContext context) : base(context)
        {
            _context = context;
        }
    }
}
