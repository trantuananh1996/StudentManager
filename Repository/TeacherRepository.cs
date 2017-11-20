using exam.Models;
using exam.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Repository
{
    public class TeacherRepository : Repository<Teacher>
    {
        public TeacherRepository(ApplicationDbContext context) : base(context)
        {
        }
        public new Task<Teacher> Get(int id)
        {
            return _context.Teachers.Where(t => t.Id == id).Include("Subject").FirstOrDefaultAsync();
        }
    }
}
