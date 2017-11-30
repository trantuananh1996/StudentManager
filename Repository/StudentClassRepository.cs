using exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentManager.Models.Post;
using Microsoft.EntityFrameworkCore;

namespace exam.Repository
{
    public class StudentClassRepository : Repository<StudentClass>
    {
        public StudentClassRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<Class>> GetAssignedClassAsync()
        {
            var cls = _context.StudentClasses.GroupBy(c => c.ClassId).ToList();
            List<Class> classes = new List<Class>();
            foreach (var stC in cls)
            {
                classes.Add(await _context.Classes.Where(c => c.Id.Equals(stC.Key)).Include("Grade").Include("Teacher").Include("SchoolYear").Include("Teacher.Subject").FirstOrDefaultAsync());
            }
            return classes;
        }
        public new async Task Create(StudentClass st)
        {
            StudentClass exist = await _context.StudentClasses.Where(s => s.ClassId == st.ClassId && s.StudentId == st.StudentId).FirstOrDefaultAsync();
            if (exist == null) await base.Create(st);
        }
        public async Task MoveClass(PostMoveClass post)
        {
            List<StudentClass> stCls = await _context.StudentClasses.Where(cl => cl.ClassId == post.SourceClass).ToListAsync();
            if (stCls.Any())
                foreach (int sId in post.StudentIds)
                {
                    var studentClass = await _context.StudentClasses.Where(s => s.ClassId == post.SourceClass && s.StudentId == sId).FirstOrDefaultAsync();
                    // var point = await _context.points.Where(s => s.Student.Id == sId && s.Class.Id == post.SourceClass).FirstOrDefaultAsync();

                    if (studentClass == null)
                    {
                        await Create(studentClass);
                    }
                    else
                    {
                        studentClass.ClassId = post.TargetClass;
                        await Update(studentClass.Id, studentClass);
                    }

                }
            else
            {
                foreach (int sId in post.StudentIds)
                {
                    await Create(new Models.StudentClass { StudentId = sId, ClassId = post.TargetClass });
                }
            }
        }
    }
}
