using exam.Models;
using exam.Repository;
using Microsoft.EntityFrameworkCore;
using StudentManager.Models;
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

        public async Task<List<Subject>> GetSubjectsAsync(int schoolYearId, int classId)
        {
            var ass = await _context.Assignments.Where(a =>
             a.SchoolYear.Id==schoolYearId
             && a.Class.Id==classId).ToListAsync();
            List<Subject> sub = new List<Subject>();
            foreach (Assignment a in ass)
            {
                sub.Add(await _context.Subjects.Where(s =>
                s.Id==a.Subject.Id).FirstOrDefaultAsync());
            }
            return sub;
        }
    }
}
