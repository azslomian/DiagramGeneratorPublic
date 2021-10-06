using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;

namespace DiagramGenerator.Domain.Repositories
{
    public class OperationRepository : GenericRepository<Operation>, IOperationRepository
    {
        private readonly DiagramGeneratorContext _context;

        public OperationRepository(DiagramGeneratorContext context) : base(context)
        {
            _context = context;
        }
    }
}
