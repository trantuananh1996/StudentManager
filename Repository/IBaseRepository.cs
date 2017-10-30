using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace exam.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task Create(TEntity o);
        Task Delete(int id);
        Task Update(int primary, TEntity o);
        Task<TEntity> Get(int id);
        Task<List<TEntity>> getAll();
        Task<List<TEntity>> paginate(int perPage,int page = 1);
    }
}
