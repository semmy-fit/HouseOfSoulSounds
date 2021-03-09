using System;
using System.Linq;

namespace HouseOfSoulSounds.Models.Domain.Repositories.Abstract
{
    public interface IPageItemsBaseRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Items { get; }
        TEntity GetItemById(Guid id);
        void SaveItem(TEntity entity);
        void DeleteItem(Guid id);
    }
}
