using System;
using exam.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Repository
{
    public class LearningCapacitiesRepository : Repository<LearningCapacities>
    {
        public LearningCapacitiesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
