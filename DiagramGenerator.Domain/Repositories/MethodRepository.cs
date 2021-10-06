using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;

namespace DiagramGenerator.Domain.Repositories
{
    public class MethodRepository : UserGenericRepository<Method>, IMethodRepository
    {
        private readonly DiagramGeneratorContext _context;

        public MethodRepository(DiagramGeneratorContext context) : base(context)
        {
            _context = context;
        }
    }
}
