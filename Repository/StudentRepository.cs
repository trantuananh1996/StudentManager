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
        public async Task<List<Student>> FindStudentByName(string name)
        {
            var students = await _context.Students.Where(s => s.FullName.Contains(name))
                //.Include("Nation")
               // .Include("Religion")
                .ToListAsync();

            return students;
        }

        public async Task<List<Student>> FindStudentByClass(int classId)
        {
            var users = await _context.StudentClasses.Where(u => u.ClassId==classId)
                            .ToListAsync();
            var students = new List<Student>();
            foreach (var cls in users)
            {
                var s = await _context.Students.FindAsync(cls.StudentId);
                if (s!=null)
                    students.Add(s);
            }
            return students;
        }
    }
}
