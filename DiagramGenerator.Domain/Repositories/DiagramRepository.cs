using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Model;
using DiagramGenerator.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramGenerator.Domain.Repositories
{
    public class DiagramRepository : UserGenericRepository<Diagram>, IDiagramRepository
    {
        private readonly DiagramGeneratorContext _context;

        public DiagramRepository(DiagramGeneratorContext context) : base(context)
        {
            _context = context;
        }

        public Diagram GetDiagramById(Guid id)
        {
            return _context.Diagram.FirstOrDefault(x => x.Id == id);
        }

        public List<Diagram> GetDiagramsByUser(string username)
        {
            return _context.Diagram.ToList();
        }

        public Diagram GetDiagramIncludeAll(Guid id)
        {
            return _context.Diagram
                .Include(x => x.Process).ThenInclude(y => y.Operations)
                .Include(x => x.Inputs).ThenInclude(y => y.Input)
                .Include(x => x.Outputs).ThenInclude(y => y.Output)
                .Include(x => x.Methods).ThenInclude(y => y.Method)
                .Include(x => x.Requirements).ThenInclude(y => y.Requirement)
                .Include(x => x.Clients).ThenInclude(y => y.Client)
                .Include(x => x.Criteria).ThenInclude(y => y.Criterion)
                .Include(x => x.Suppliers).ThenInclude(y => y.Supplier)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
