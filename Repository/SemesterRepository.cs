using exam.Models;
using exam.Repository;
using Microsoft.EntityFrameworkCore;
using StudentManager.Models.Point;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace StudentManager.Repository
{
    public class SemesterRepository : Repository<Semester>
    {
        public SemesterRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<Conduct> FindByName(string name)
        {
            return await _context.Conducts.Where(c => c.Name.Equals(name)).FirstOrDefaultAsync();
        }

        internal Task GetSemester()
        {
            throw new NotImplementedException();
        }
    }
}
