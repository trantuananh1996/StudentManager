using System;
using exam.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace exam.Repository
{
    public class LearningCapacitiesRepository : Repository<LearningCapacities>
    {
        public LearningCapacitiesRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<Conduct> FindByName(string name)
        {
            return await _context.Conducts.Where(c => c.Name.Equals(name)).FirstOrDefaultAsync();
        }
    }
}
