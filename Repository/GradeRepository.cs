using exam.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Repository
{
    public class GradeRepository : Repository<Grade>
    {
        public GradeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Conduct> FindByName(string name)
        {
            return await _context.Conducts.Where(c => c.Name.Equals(name)).FirstOrDefaultAsync();
        }
    }
}
