using exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Repository
{
    public class NationRepository : Repository<Nation>
    {
        public NationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
