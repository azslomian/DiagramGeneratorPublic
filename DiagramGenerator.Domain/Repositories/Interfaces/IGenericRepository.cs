using DiagramGenerator.DataAccess.Abstract;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiagramGenerator.Domain.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity>
        where TEntity : Entity
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetById(Guid id);

        Task<TEntity> GetLast();

        Task Create(TEntity entity);

        Task Update(Guid id, TEntity entity);

        Task Delete(Guid id);

        Task SaveChangesAsync();

        void SaveChanges();

        TEntity GetByIdEager(Guid id);

        Task<TEntity> GetByIdTracking(Guid id);

        Task UpdateCollectionElement(Guid id, TEntity entity);

        void ChangeTrackerClear();
    }
}
