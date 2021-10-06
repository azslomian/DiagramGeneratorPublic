using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;

namespace DiagramGenerator.Domain.Repositories
{
    public class RequirementRepository : UserGenericRepository<Requirement>, IRequirementRepository
    {
        private readonly DiagramGeneratorContext _context;

        public RequirementRepository(DiagramGeneratorContext context) : base(context)
        {
            _context = context;
        }
    }
}
