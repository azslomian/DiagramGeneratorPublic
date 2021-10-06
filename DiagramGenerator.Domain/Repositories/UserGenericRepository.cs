using DiagramGenerator.DataAccess;
using DiagramGenerator.DataAccess.Abstract;
using DiagramGenerator.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class UserGenericRepository<TUserEntity> : IUserGenericRepository<TUserEntity>
    where TUserEntity : UserEntity
{
    private readonly DiagramGeneratorContext _context;

    public UserGenericRepository(DiagramGeneratorContext context)
    {
        _context = context;
    }

    public IQueryable<TUserEntity> GetAll(string email)
    {
        return _context.Set<TUserEntity>().Where(x => x.UserEmail == email).OrderBy(x => x.Lp).AsNoTracking();
    }

    public async Task<TUserEntity> GetById(Guid id)
    {
        return await _context.Set<TUserEntity>()
                    .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<TUserEntity> GetLast(string email)
    {
        return _context.Set<TUserEntity>().AsNoTracking().OrderByDescending(p => p.Lp)
                       .FirstOrDefault();
    }

    public async Task<TUserEntity> GetByIdTracking(Guid id)
    {
        return await _context.Set<TUserEntity>()
                    .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task Create(TUserEntity entity)
    {
        await _context.Set<TUserEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Guid id, TUserEntity entity)
    {
        _context.Set<TUserEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCollectionElement(Guid id, TUserEntity entity)
    {
        _context.Set<TUserEntity>().Attach(entity);
        _context.Entry(entity).Property(x => x.Description).IsModified = true;
        _context.Entry(entity).Property(x => x.Name).IsModified = true;
        _context.Set<TUserEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var entity = await GetById(id);
        _context.Set<TUserEntity>().Remove(entity);
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

    public TUserEntity GetByIdEager(Guid id)
    {
        return Query().FirstOrDefault(i => i.Id == id);
    }

    private IQueryable<TUserEntity> Query()
    {
        var query = _context.Set<TUserEntity>().AsQueryable();

        var navigations = _context.Model.FindEntityType(typeof(TUserEntity))
            .GetDerivedTypesInclusive()
            .SelectMany(type => type.GetNavigations())
            .Distinct();

        foreach (var property in navigations)
            query = query.Include(property.Name);

        return query;
    }

}