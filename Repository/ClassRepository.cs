using exam.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Repository
{
    public class ClassRepository : Repository<Class>
    {
        public ClassRepository(ApplicationDbContext context) : base(context)
        {
        }

        public new async Task<List<Class>> GetAll()
        {
            return await _context.Set<Class>().Include("Grade").Include("Teacher").Include("SchoolYear").Include("Teacher.Subject")
                .ToListAsync();
        }
        public new async Task<Class> Get(int id)
        {
            return await _context.Set<Class>().Where(c=>c.Id.Equals(id)).Include("Grade").Include("Teacher").Include("SchoolYear")
                .Include("Teacher.Subject")
                .FirstOrDefaultAsync();
        }
    }
}
