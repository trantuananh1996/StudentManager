using StudentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam.Models;
using exam.Repository;
using Microsoft.EntityFrameworkCore;

namespace exam.Repository
{
    public class AssignmentRepository : Repository<Assignment>
    {
        public AssignmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Subject>> GetSubject(int schoolyearid, int classid)
        {
            List<Subject> subjects = new List<Subject>();
            var listAssign = await _context.Assignments
                .Where(a => a.SchoolYear.Id==schoolyearid
                && a.Class.Id==classid).ToListAsync();
            if (listAssign != null && listAssign.Any())
            {
                foreach(Assignment a in listAssign)
                {
                    subjects.Add(await _context.Subjects.Where(s=>s.Id==a.Subject.Id).FirstOrDefaultAsync());
                }
            }
            return subjects;

        }
        public new async Task<Assignment> Get(int id)
        {
            return await _context.Set<Assignment>().Where(a=>a.Id==id).Include("SchoolYear")
                .Include("Class")
                .Include("Subject")
                .Include("Teacher").FirstOrDefaultAsync();
        }
        public new async Task<List<Assignment>> GetAll()
        {
            return await _context.Set<Assignment>().Include("SchoolYear")
                .Include("Class")
                .Include("Subject")
                .Include("Teacher")
                .ToListAsync();
        }
    }
}
