using exam.Repository;
using StudentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManager.Repository
{
    public class RuleRepository : Repository<Rule>
    {
        public RuleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Models.Rule> GetDefaultRule()
        {
            var rule= await _context.Rules.FirstOrDefaultAsync();
           
            return rule;
        }
    }
}
