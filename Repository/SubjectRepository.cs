using exam.Models;
using exam.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Repository
{
    public class SubjectRepository : Repository<Subject>
    {
        public SubjectRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
