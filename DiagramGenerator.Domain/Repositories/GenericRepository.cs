using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Abstract;
using DiagramGenerator.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : Entity
{
    private readonly DiagramGeneratorContext _context;

    public GenericRepository(DiagramGeneratorContext context)
    {
        _context = context;
    }

    public IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().OrderBy(x => x.Lp).AsNoTracking();
    }

    public async Task<TEntity> GetById(Guid id)
    {
        return await _context.Set<TEntity>()
                    .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<TEntity> GetLast()
    {
        return _context.Set<TEntity>().AsNoTracking().OrderByDescending(p => p.Lp)
                       .FirstOrDefault();
    }

    public async Task<TEntity> GetByIdTracking(Guid id)
    {
        return await _context.Set<TEntity>()
                    .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task Create(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Guid id, TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCollectionElement(Guid id, TEntity entity)
    {
        _context.Set<TEntity>().Attach(entity);
        _context.Entry(entity).Property(x => x.Description).IsModified = true;
        _context.Entry(entity).Property(x => x.Name).IsModified = true;
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var entity = await GetById(id);
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void ChangeTrackerClear()
    {
        _context.ChangeTracker.Clear();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public TEntity GetByIdEager(Guid id)
    {
        return Query().FirstOrDefault(i => i.Id == id);
    }

    private IQueryable<TEntity> Query()
    {
        var query = _context.Set<TEntity>().AsQueryable();

        var navigations = _context.Model.FindEntityType(typeof(TEntity))
            .GetDerivedTypesInclusive()
            .SelectMany(type => type.GetNavigations())
            .Distinct();

        foreach (var property in navigations)
            query = query.Include(property.Name);

        return query;
    }

}