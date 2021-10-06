using DiagramGenerator.DataAccess.Abstract;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiagramGenerator.Domain.Repositories.Interfaces
{
    public interface IUserGenericRepository<TUserEntity>
        where TUserEntity : UserEntity
    {
        IQueryable<TUserEntity> GetAll(string email);

        Task<TUserEntity> GetById(Guid id);

        Task<TUserEntity> GetLast(string email);

        Task Create(TUserEntity entity);

        Task Update(Guid id, TUserEntity entity);

        Task Delete(Guid id);

        Task SaveChangesAsync();

        void SaveChanges();

        TUserEntity GetByIdEager(Guid id);

        Task<TUserEntity> GetByIdTracking(Guid id);

        Task UpdateCollectionElement(Guid id, TUserEntity entity);

        void ChangeTrackerClear();
    }
}
