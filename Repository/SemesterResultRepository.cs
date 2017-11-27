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
    public class SemesterResultRepository : RepositoryNoId<SemesterResult>
    {
        public SemesterResultRepository(ApplicationDbContext context) : base(context)
        {
        }



        public async void Save(PointController pointController, ConductRepository conductRepository, int studentId, int classId, int semesterId, int schoolYearId)
        {
            float diemTBChungCacMonHK = (float)Math.Round(await pointController.DiemTrungBinhChungCacMonHocKy(studentId, classId, semesterId, schoolYearId), 2);
            int hocLuc =await pointController.XepLoaiLocLucHocKyAsync(studentId, classId, semesterId, schoolYearId);

            var exist = await _context.SemesterResults.Where(s =>
           s.StudentId.Equals(studentId)
           && s.ClassId.Equals(classId)
           && s.SemesterId.Equals(semesterId)
           && s.SchoolYearId.Equals(schoolYearId)
         ).FirstOrDefaultAsync();
            exist.AverageSubjectScore = diemTBChungCacMonHK;
            exist.LearningCapacityId = hocLuc;

            await _context.SaveChangesAsync();
        }
    }
}
