using exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Repository
{
    public class ConductRepository : Repository<Nation>
    {
        public ConductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
