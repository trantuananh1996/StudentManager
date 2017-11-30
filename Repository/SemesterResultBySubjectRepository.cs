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
    public class SemesterResultBySubjectRepository : Repository<SemesterResultBySubject>
    {
        public SemesterResultBySubjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<SemesterResultBySubject> Find(PointController pointController, int studentId, int classId, int subjectId, int semesterId, int schoolYearId)
        {
                 return await _context.SemesterResultBySubjects.Where(s =>
               s.StudentId.Equals(studentId)
               && s.ClassId.Equals(classId)
               && s.SubjectId.Equals(subjectId)
               && s.SemesterId.Equals(semesterId)
               && s.SchoolYearId.Equals(schoolYearId)
             ).FirstOrDefaultAsync();
        
        }

        public async Task Save(PointController pointController, int studentId, int classId, int subjectId, int semesterId, int schoolYearId)
        {
            float diemTBKT = (float)Math.Round(await pointController.DiemTrungBinhKiemTra(studentId, subjectId, semesterId, schoolYearId, classId), 2);
            float diemTBMonHK = (float)Math.Round(await pointController.DiemTrungBinhMonHocKy(studentId, subjectId, semesterId, schoolYearId, classId), 2);

           var exist=await _context.SemesterResultBySubjects.Where(s =>
            s.StudentId.Equals(studentId)
            && s.ClassId.Equals(classId)
            && s.SubjectId.Equals(subjectId)
            && s.SemesterId.Equals(semesterId)
            && s.SchoolYearId.Equals(schoolYearId)
            ).FirstOrDefaultAsync();
            if (exist == null)
            {
                exist = new SemesterResultBySubject
                {
                    StudentId = studentId,
                    ClassId = classId,
                    SubjectId = subjectId,
                    SemesterId = semesterId,
                    SchoolYearId = schoolYearId,
                    AverageExamScore = diemTBKT,
                    AverageSubjectScore = diemTBMonHK
                };
               
                await Create(exist);
                return;
            }
            exist.AverageExamScore = diemTBKT;
            exist.AverageSubjectScore = diemTBMonHK;
            await _context.SaveChangesAsync();
        }
    }
}
