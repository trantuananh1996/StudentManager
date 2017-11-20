using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace exam.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Create(TEntity o);
        Task Delete(int id);
        Task<TEntity> Update(int primary, TEntity o);

        Task<TEntity> Get(int id);
        Task<List<TEntity>> GetAll();
        Task<List<TEntity>> Paginate(int perPage,int page = 1);
    }
}
