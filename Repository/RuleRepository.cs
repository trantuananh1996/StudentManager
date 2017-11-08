using exam.Repository;
using StudentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam.Models;

namespace StudentManager.Repository
{
    public class RuleRepository : Repository<Rule>, IRuleRepository
    {
        public RuleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
