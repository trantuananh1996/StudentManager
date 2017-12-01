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
            p.SchoolYear.Id==given.SchoolYearId
            && p.Semester.Id==given.SemesterId
            && p.Class.Id==given.ClassId
            && p.Subject.Id==given.SubjectId
            && p.Student.Id==given.StudentId
            ).Include("SchoolYear")
            .Include("Semester")
            .Include("Class")
            .Include("Student")
            .Include("Subject")
            .Include("PointType")
            .ToListAsync();
        }
        public async Task<List<Models.Point.Point>> GetStudentPoint(int maHocSinh, int maMonHoc, int maHocKy, int maNamHoc, int maLop)
        {
            return await _context.Points.Where(p =>
            p.SchoolYear.Id==maNamHoc
            && p.Semester.Id==maHocKy
            && p.Class.Id==maLop
            && p.Subject.Id==maMonHoc
            && p.Student.Id==maHocSinh
            ).Include("SchoolYear")
            .Include("Semester")
            .Include("Class")
            .Include("Student")
            .Include("Subject")
            .Include("PointType")
            .ToListAsync();
        }
    }
}
