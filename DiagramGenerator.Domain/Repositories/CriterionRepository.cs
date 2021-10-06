using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;

namespace DiagramGenerator.Domain.Repositories
{
    public class CriterionRepository : UserGenericRepository<Criterion>, ICriterionRepository
    {
        private readonly DiagramGeneratorContext _context;

        public CriterionRepository(DiagramGeneratorContext context) : base(context)
        {
            _context = context;
        }
    }
}
