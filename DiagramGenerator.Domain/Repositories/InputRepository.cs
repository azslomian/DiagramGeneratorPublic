using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;

namespace DiagramGenerator.Domain.Repositories
{
    public class InputRepository : UserGenericRepository<Input>, IInputRepository
    {
        private readonly DiagramGeneratorContext _context;

        public InputRepository(DiagramGeneratorContext context) : base(context)
        {
            _context = context;
        }
    }
}
