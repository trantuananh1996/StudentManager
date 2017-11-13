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

        public new async Task<List<Class>> getAll()
        {
            return await _context.Set<Class>().Include("Grade").Include("Teacher").Include("SchoolYear")
                .Include("Teacher.Subject").ToListAsync();
        }
    }
}
