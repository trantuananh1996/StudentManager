using exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Repository
{
    public class GradeRepository : Repository<Grade>, IGradeRepository
    {
        public GradeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
