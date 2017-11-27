using exam.Repository;
using StudentManager.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam.Models;
using StudentManager.Controllers;
using Microsoft.EntityFrameworkCore;

namespace StudentManager.Repository
{
    public class YearResultBySubjectRepository : RepositoryNoId<YearResultBySubject>
    {
        public YearResultBySubjectRepository(ApplicationDbContext context) : base(context)
        {
        }



        public async void Save(PointController pointController, int studentId, int classId, int subjectId, int schoolYearId)
        {
            float diemTBMonCN = (float)Math.Round(await pointController.DiemTrungBinhMonCaNam(studentId, subjectId, schoolYearId, classId), 2);
            float diemThiLai = 0;

            var exist = await _context.YearResultSubjects.Where(s =>
              s.StudentId.Equals(studentId)
              && s.ClassId.Equals(classId)
              && s.SubjectId.Equals(subjectId)
               && s.SchoolYearId.Equals(schoolYearId)
             ).FirstOrDefaultAsync();
            exist.ReExamScore = diemThiLai;
            exist.AverageScore = diemTBMonCN;

            await _context.SaveChangesAsync();
        }
    }
}
