using exam.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam.Models;
using StudentManager.Models.Point;
using Microsoft.EntityFrameworkCore;

namespace StudentManager.Repository.Point
{
    public class PointRepository : Repository<Models.Point.Point>
    {
        public PointRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Models.Point.Point>> GetStudentPoint(PostUpdatePoint given)
        {
            return await _context.Points.Where(p =>
            p.SchoolYear.Id.Equals(given.SchoolYearId)
            && p.Semester.Id.Equals(given.SemesterId)
            && p.Class.Id.Equals(given.ClassId)
            && p.Subject.Id.Equals(given.SubjectId)
            && p.Student.Id.Equals(given.StudentId)
            ).Include("SchoolYear")
            .Include("Semester")
            .Include("Class")
            .Include("Student")
            .Include("Subject")
            .ToListAsync();
        }
        public async Task<List<Models.Point.Point>> GetStudentPoint(int maHocSinh, int maMonHoc, int maHocKy, int maNamHoc, int maLop)
        {
            return await _context.Points.Where(p =>
            p.SchoolYear.Id.Equals(maNamHoc)
            && p.Semester.Id.Equals(maHocKy)
            && p.Class.Id.Equals(maLop)
            && p.Subject.Id.Equals(maMonHoc)
            && p.Student.Id.Equals(maHocSinh)
            ).Include("SchoolYear")
            .Include("Semester")
            .Include("Class")
            .Include("Student")
            .Include("Subject")
            .ToListAsync();
        }
    }
}
