using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;

namespace DiagramGenerator.Domain.Repositories
{
    public class SupplierRepository : UserGenericRepository<Supplier>, ISupplierRepository
    {
        private readonly DiagramGeneratorContext _context;

        public SupplierRepository(DiagramGeneratorContext context) : base(context)
        {
            _context = context;
        }
    }
}
