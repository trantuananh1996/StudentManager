using exam.Models;
using exam.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Repository
{
    public class JobRepository : Repository<Job>
    {
        public JobRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Conduct> FindByName(string name)
        {
            return await _context.Conducts.Where(c => c.Name.Equals(name)).FirstOrDefaultAsync();
        }
    }
}
