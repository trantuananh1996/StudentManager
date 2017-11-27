using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace exam.Repository
{
    public class RepositoryNoId<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        public RepositoryNoId(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TEntity> Create(TEntity o)
        {

            await _context.Set<TEntity>().AddAsync(o);
            await _context.SaveChangesAsync();

   
            return o;

        }

        public async Task Delete(int id)
        {
            var itemToDelete = await _context.Set<TEntity>().FindAsync(id);
            if (itemToDelete != null)
            {
                _context.Remove(itemToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TEntity> Get(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        public static object ToType<T>(object obj, T type)
        {
            //create instance of T type object:
            object tmp = Activator.CreateInstance(Type.GetType(type.ToString()));

            //loop through the properties of the object you want to covert:          
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                try
                {
                    //get the value of property and try to assign it to the property of T type object:
                    tmp.GetType().GetProperty(pi.Name).SetValue(tmp, pi.GetValue(obj, null), null);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    //Logging.Log.Error(ex);
                }
            }
            //return the T type object:         
            return tmp;
        }
        public async Task<TEntity> Update(int id, TEntity o)
        {
            var itemToUpdate = await _context.Set<TEntity>().FindAsync(id);
            if (itemToUpdate != null)
            {
                itemToUpdate = o;
                await _context.SaveChangesAsync();
            }
            return itemToUpdate;
        }

        public async Task<List<TEntity>> Paginate(int perPage, int page)
        {
            return await _context.Set<TEntity>().Take(perPage)
                                 .Skip((page - 1) * perPage)
                                 .ToListAsync();
        }
    }
}
