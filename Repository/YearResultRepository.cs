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
    public class YearResultRepository : RepositoryNoId<YearResult>
    {
        public YearResultRepository(ApplicationDbContext context) : base(context)
        {
        }



        public async void Save(PointController pointController, int studentId, int classId, int schoolYearId)
        {
            float diemTBChungCacMonCN = (float)Math.Round(await pointController.DiemTrungBinhChungCacMonCaNam(studentId, classId, schoolYearId), 2);
            int hocLuc = await pointController.XepLoaiLocLucCaNamAsync(studentId, classId, schoolYearId);

            var exist = await _context.YearResults.Where(s =>
             s.StudentId.Equals(studentId)
             && s.ClassId.Equals(classId)
             && s.SchoolYearId.Equals(schoolYearId)
           ).FirstOrDefaultAsync();
            exist.AverageScore = diemTBChungCacMonCN;
            exist.LearningCapacityId = hocLuc;
            exist.ResultId = 1;

            await _context.SaveChangesAsync();
        }
    }
}
