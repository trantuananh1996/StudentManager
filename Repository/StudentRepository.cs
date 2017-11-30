using exam.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Repository
{
    public class StudentRepository : Repository<Student>
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public new async Task<List<Student>> GetAll()
        {
            return await _context.Students.Include("Nation").Include("Religion").Include("FatherJob").Include("MotherJob").ToListAsync();
        }
        public new async Task<Student> Get(int id)
        {
            return await _context.Students.Where(st => st.Id.Equals(id)).Include("Nation").Include("Religion").Include("FatherJob").Include("MotherJob").FirstOrDefaultAsync();
        }
        public async Task<List<Student>> FindStudentByName(string name)
        {
            var students = await _context.Students.Where(s => s.FullName.Contains(name))
                .Include("Nation")
                .Include("Religion")
                .Include("FatherJob")
                .Include("MotherJob")
                .ToListAsync();

            return students;
        }

        public async Task<List<Student>> FindStudentByClass(int classId)
        {
            var users = await _context.StudentClasses.Where(u => u.ClassId == classId)
                            .ToListAsync();
            var students = new List<Student>();
            foreach (var cls in users)
            {
                var s = await _context.Students.Where(st => st.Id.Equals(cls.StudentId)).Include("Nation")
                .Include("Religion")
                .Include("FatherJob")
                .Include("MotherJob").FirstOrDefaultAsync();
                if (s != null)
                    students.Add(s);
            }
            return students;
        }
    }
}
