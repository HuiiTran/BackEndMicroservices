using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServicesCommon
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T entity);

        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);//filter for later use 

        Task<T> GetAsync(Guid id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);

        Task RemoveAsync(Guid id);

        Task UpdateAsync(T entity);
    }
}
