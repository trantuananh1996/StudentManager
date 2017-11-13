using exam.Models;
using exam.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Repository
{
    public class SchoolYearRepository : Repository<SchoolYear>
    {
        public SchoolYearRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
