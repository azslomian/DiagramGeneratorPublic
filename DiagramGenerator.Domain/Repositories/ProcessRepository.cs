using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DiagramGenerator.Domain.Repositories
{
    public class ProcessRepository : GenericRepository<Process>, IProcessRepository
    {
        private readonly DiagramGeneratorContext _context;

        public ProcessRepository(DiagramGeneratorContext context) : base(context)
        {
            _context = context;
        }

        public Process GetProcessByDiagramId(Guid diagramId)
        {
            return _context.Process
                .Include(x => x.Operations)
                .AsNoTracking()
                .FirstOrDefault(process => process.DiagramId == diagramId);
        }

        public Process GetProcessById(Guid id)
        {
            return _context.Process
                .Include(x => x.Operations)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
