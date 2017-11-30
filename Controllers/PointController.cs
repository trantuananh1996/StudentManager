using exam.Models;
using exam.Repository;
using exam.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models;
using StudentManager.Models.Point;
using StudentManager.Models.Reports;
using StudentManager.Repository;
using StudentManager.Repository.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Controllers
{
    [Route("api/point")]
    public class PointController : Controller
    {
        static float maxPoint = 0;
        PointTypeRepository pointTypeRepository;
        PointRepository pointRepository;
        RuleRepository ruleRepository;
        StudentRepository studentRepository;
        SubjectRepository subjectRepository;
        SemesterRepository semesterRepository;
        SchoolYearRepository schoolYearRepository;
        ClassRepository classRepository;
        LearningCapacitiesRepository learningCapacitiesRepository;
        AssignmentRepository assignmentRepository;
        SemesterResultRepository semesterResultRepository;
        SemesterResultBySubjectRepository semesterResultBySubjectRepository;
        YearResultRepository yearResultRepository;
        YearResultBySubjectRepository yearResultBySubjectRepository;
        ConductRepository conductRepository;
        public async Task<int> XepLoaiLocLucCaNamAsync(int studentId, int classId, int schoolYearId)
        {
            float tongDiem = 0;
            float tongDiemCacMon = 0;
            float diemTBTungMon = 0;
            int tongHeSoCacMon = 0;

            var m_DT = await subjectRepository.GetSubjectsAsync(schoolYearId, classId);

            float[] arrayDiemTBTungMon = new float[m_DT.Count];

            int soMonHoc = 0;
            foreach (Subject row in m_DT)
            {
                diemTBTungMon = await DiemTrungBinhMonCaNam(studentId, row.Id, schoolYearId, classId);
                arrayDiemTBTungMon[soMonHoc++] = diemTBTungMon;

                tongDiemCacMon += diemTBTungMon * row.Factor;
                tongHeSoCacMon += row.Factor;
            }
            if (tongHeSoCacMon > 0)
                tongDiem = tongDiemCacMon / tongHeSoCacMon;
            else
                tongDiem = 0;

            return await XepLoaiHocLucMonHocAsync(arrayDiemTBTungMon, tongDiem);
        }

        public async Task<int> XepLoaiHocLucMonHocAsync(float[] arrayDiemTBTungMon, float tongDiem)
        {
            int xepLoai = -1;
            float diemTBMonNhoNhat = arrayDiemTBTungMon.Count()==0?0:arrayDiemTBTungMon[0];

            for (int i = 0; i < arrayDiemTBTungMon.Length - 1; i++)
            {
                if (arrayDiemTBTungMon[i] < diemTBMonNhoNhat)
                    diemTBMonNhoNhat = arrayDiemTBTungMon[i];
            }

            var lCapacities = await learningCapacitiesRepository.GetAll();
            int[] maHocLuc = new int[lCapacities.Count];
            float[] diemCanDuoi = new float[lCapacities.Count];

            int count = 0;
            foreach (LearningCapacity row in lCapacities)
            {
                maHocLuc[count] = row.Id;
                diemCanDuoi[count] = row.MinPoint;
                count++;
            }

            for (int i = 0; i < count - 1; i++)
            {
                if (tongDiem >= diemCanDuoi[i] && diemTBMonNhoNhat >= diemCanDuoi[i + 1])
                {
                    xepLoai = maHocLuc[i];
                    break;
                }
            }

            if (xepLoai == -1)
                xepLoai = maHocLuc[count - 1];
            return xepLoai;
        }
        public async Task<int> XepLoaiLocLucHocKyAsync(int studentId, int classId, int semesterId, int schoolYearId)
        {
            float tongDiem = 0;
            float tongDiemCacMon = 0;
            float diemTBTungMon = 0;
            int tongHeSoCacMon = 0;

            List<Subject> Subjects = await subjectRepository.GetSubjectsAsync(schoolYearId, classId);

            float[] arrayDiemTBTungMon = new float[Subjects.Count];

            int soMonHoc = 0;
            foreach (Subject row in Subjects)
            {
                diemTBTungMon = await DiemTrungBinhMonHocKy(studentId, row.Id, semesterId, schoolYearId, classId);
                arrayDiemTBTungMon[soMonHoc++] = diemTBTungMon;

                tongDiemCacMon += diemTBTungMon * row.Factor;
                tongHeSoCacMon += row.Factor;
            }
            if (tongHeSoCacMon > 0)
                tongDiem = tongDiemCacMon / tongHeSoCacMon;
            else
                tongDiem = 0;

            return await XepLoaiHocLucMonHocAsync(arrayDiemTBTungMon, tongDiem);
        }
        public PointController(PointTypeRepository pointTypeRepository, PointRepository pointRepository, RuleRepository ruleRepository, StudentRepository studentRepository, SubjectRepository subjectRepository, SemesterRepository semesterRepository, SchoolYearRepository schoolYearRepository, ClassRepository classRepository, LearningCapacitiesRepository learningCapacitiesRepository, AssignmentRepository assignmentRepository, SemesterResultRepository semesterResultRepository, SemesterResultBySubjectRepository semesterResultBySubjectRepository, YearResultRepository yearResultRepository, YearResultBySubjectRepository yearResultBySubjectRepository, ConductRepository conductRepository)
        {
            this.pointTypeRepository = pointTypeRepository;
            this.pointRepository = pointRepository;
            this.ruleRepository = ruleRepository;
            this.studentRepository = studentRepository;
            this.subjectRepository = subjectRepository;
            this.semesterRepository = semesterRepository;
            this.schoolYearRepository = schoolYearRepository;
            this.classRepository = classRepository;
            this.learningCapacitiesRepository = learningCapacitiesRepository;
            this.assignmentRepository = assignmentRepository;
            this.semesterResultRepository = semesterResultRepository;
            this.semesterResultBySubjectRepository = semesterResultBySubjectRepository;
            this.yearResultRepository = yearResultRepository;
            this.yearResultBySubjectRepository = yearResultBySubjectRepository;
            this.conductRepository = conductRepository;
        }

        #region Danh sách loại điểm(Kiểm tra miệng, 15p, 1 tiết, học kì...)
        [Authorize(Roles = "SchoolBoard")]
        [HttpGet("point-types")]
        public async Task<ActionResult> GetPointTypes()
        {
            List<PointType> pointtypes = await pointTypeRepository.GetAll();
            if (pointtypes == null || pointtypes.Count() == 0)
                return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không có loại điểm nào" });
            else
                return Ok(new { status = ResultStatus.STATUS_OK, data = pointtypes });
        }
        #endregion

        private bool IsPointInvalid(float point, float maxPoint)
        {
            if (maxPoint == default(float)) maxPoint = 10;
            if (point < 0 || point > maxPoint) return true;
            else return false;
        }

        [HttpPost("update-point")]
        public async Task CreateOrUpdate([FromBody] PostUpdatePoint givenPoint)
        {
            foreach (ListPoint ls in givenPoint.ListPoints)
            {
                if (ls.Id == default(int))
                {
                    #region Tạo mới điểm
                    Point point = new Point
                    {
                        Student = await studentRepository.Get(givenPoint.StudentId),
                        Subject = await subjectRepository.Get(givenPoint.SubjectId),
                        Semester = await semesterRepository.Get(givenPoint.SemesterId),
                        SchoolYear = await schoolYearRepository.Get(givenPoint.SchoolYearId),
                        Class = await classRepository.Get(givenPoint.ClassId),
                        PointType = await pointTypeRepository.Get(ls.PointTypeId),
                        PointNumber = ls.Point
                    };
                    await pointRepository.Create(point);

                    #endregion
                }
                else
                {
                    #region Sửa điểm
                    var existPoint = await pointRepository.Get(ls.Id);
                    if (existPoint == null)
                    {
                        Point point = new Point
                        {
                            Student = await studentRepository.Get(givenPoint.StudentId),
                            Subject = await subjectRepository.Get(givenPoint.SubjectId),
                            Semester = await semesterRepository.Get(givenPoint.SemesterId),
                            SchoolYear = await schoolYearRepository.Get(givenPoint.SchoolYearId),
                            Class = await classRepository.Get(givenPoint.ClassId),
                            PointType = await pointTypeRepository.Get(ls.PointTypeId),
                            PointNumber = ls.Point
                        };
                        await pointRepository.Create(point);

                    }
                    else
                    {
                        existPoint.Student = await studentRepository.Get(givenPoint.StudentId);
                        existPoint.Subject = await subjectRepository.Get(givenPoint.SubjectId);
                        existPoint.Semester = await semesterRepository.Get(givenPoint.SemesterId);
                        existPoint.SchoolYear = await schoolYearRepository.Get(givenPoint.SchoolYearId);
                        existPoint.Class = await classRepository.Get(givenPoint.ClassId);
                        existPoint.PointType = await pointTypeRepository.Get(ls.PointTypeId);
                        existPoint.PointNumber = ls.Point;

                    }
                    #endregion
                }
            }
           await semesterResultBySubjectRepository.Save(this, givenPoint.StudentId,
                                                     givenPoint.ClassId,
                                                    givenPoint.SubjectId,
                                                   givenPoint.SemesterId,
                                                   givenPoint.SchoolYearId);

            await yearResultBySubjectRepository.Save(this, givenPoint.StudentId,
                                                     givenPoint.ClassId,
                                                    givenPoint.SubjectId,
                                                   givenPoint.SchoolYearId);

            await semesterResultRepository.Save(this, conductRepository, givenPoint.StudentId,
                                            givenPoint.ClassId,
                                          givenPoint.SemesterId,
                                          givenPoint.SchoolYearId);

            await yearResultRepository.Save(this, givenPoint.StudentId,
                                             givenPoint.ClassId,
                                           givenPoint.SchoolYearId);

        }
        [HttpPost("update")]
        public async Task<ActionResult> CreateOrUpdate([FromBody] List<PostUpdatePoint> points)
        {
            if (PointController.maxPoint == default(float))
            {
                Rule rule = await ruleRepository.GetDefaultRule();
                maxPoint = rule.MaxPoint == default(float) ? 0 : rule.MaxPoint;
            }
            foreach (PostUpdatePoint givenPoint in points)
            {
                foreach (ListPoint ls in givenPoint.ListPoints)
                {
                    if (ls.Id == default(int))
                    {
                        #region Tạo mới điểm
                        Point point = new Point
                        {
                            Student = await studentRepository.Get(givenPoint.StudentId),
                            Subject = await subjectRepository.Get(givenPoint.SubjectId),
                            Semester = await semesterRepository.Get(givenPoint.SemesterId),
                            SchoolYear = await schoolYearRepository.Get(givenPoint.SchoolYearId),
                            Class = await classRepository.Get(givenPoint.ClassId),
                            PointType = await pointTypeRepository.Get(ls.PointTypeId),
                            PointNumber = ls.Point
                        };
                        await pointRepository.Create(point);

                        #endregion
                    }
                    else
                    {
                        #region Sửa điểm
                        var existPoint = await pointRepository.Get(ls.Id);
                        if (existPoint == null)
                        {
                            Point point = new Point
                            {
                                Student = await studentRepository.Get(givenPoint.StudentId),
                                Subject = await subjectRepository.Get(givenPoint.SubjectId),
                                Semester = await semesterRepository.Get(givenPoint.SemesterId),
                                SchoolYear = await schoolYearRepository.Get(givenPoint.SchoolYearId),
                                Class = await classRepository.Get(givenPoint.ClassId),
                                PointType = await pointTypeRepository.Get(ls.PointTypeId),
                                PointNumber = ls.Point
                            };
                            await pointRepository.Create(point);

                        }
                        else
                        {
                            existPoint.Student = await studentRepository.Get(givenPoint.StudentId);
                            existPoint.Subject = await subjectRepository.Get(givenPoint.SubjectId);
                            existPoint.Semester = await semesterRepository.Get(givenPoint.SemesterId);
                            existPoint.SchoolYear = await schoolYearRepository.Get(givenPoint.SchoolYearId);
                            existPoint.Class = await classRepository.Get(givenPoint.ClassId);
                            existPoint.PointType = await pointTypeRepository.Get(ls.PointTypeId);
                            existPoint.PointNumber = ls.Point;

                        }
                        #endregion
                    }
                }

                await semesterResultBySubjectRepository.Save(this, givenPoint.StudentId,
                                                      givenPoint.ClassId,
                                                     givenPoint.SubjectId,
                                                    givenPoint.SemesterId,
                                                    givenPoint.SchoolYearId);

                await yearResultBySubjectRepository.Save(this, givenPoint.StudentId,
                                                    givenPoint.ClassId,
                                                   givenPoint.SubjectId,
                                                  givenPoint.SchoolYearId);

                await semesterResultRepository.Save(this, conductRepository, givenPoint.StudentId,
                                                givenPoint.ClassId,
                                              givenPoint.SemesterId,
                                              givenPoint.SchoolYearId);

                await yearResultRepository.Save(this, givenPoint.StudentId,
                                                 givenPoint.ClassId,
                                               givenPoint.SchoolYearId);




            }



            return Ok(new { status = ResultStatus.STATUS_OK, data = points });
        }

        /**
         * Lấy danh sách điểm của học sinh
         * 
         * */
        [HttpPost("showPoint")]
        public async Task<ActionResult> ShowStudentPoint([FromBody] PostUpdatePoint given)//Không dùng PointNumber
        {
            var list = await pointRepository.GetStudentPoint(given);
            if (list == null || !list.Any())
                return NotFound(new { message = "Học sinh chưa có điểm" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = list });
        }

        #region Tính các loại điểm
        public async Task<float> DiemTrungBinhKiemTra(int studentId, int maMonHoc, int semesterId, int schoolYearId, int classId)
        {
            var list = await pointRepository.GetStudentPoint(studentId, maMonHoc, semesterId, schoolYearId, classId);
            if (list != null && list.Any())
            {
                float tongDiem = 0;
                int tongHeSo = 0;

                foreach (Point p in list)
                {
                    if (p.PointType.Id != 4)//Không phải điểm thi học kì
                    {
                        tongDiem += p.PointNumber * p.PointType.Factor;
                        tongHeSo += p.PointType.Factor;
                    }
                }

                if (tongHeSo > 0)
                    return tongDiem / tongHeSo;
                else
                    return 0;
            }
            return 0;
        }
        public async Task<float> DiemTrungBinhMonHocKy(int studentId, int maMonHoc, int semesterId, int schoolYearId, int classId)
        {
            var list = await pointRepository.GetStudentPoint(studentId, maMonHoc, semesterId, schoolYearId, classId);
            if (list != null && list.Any())
            {
                float tongDiem = 0;
                int tongHeSo = 0;

                foreach (Point p in list)
                {
                    tongDiem += p.PointNumber * p.PointType.Factor;
                    tongHeSo += p.PointType.Factor;
                }

                if (tongHeSo > 0)
                    return tongDiem / tongHeSo;
                else
                    return 0;
            }
            return 0;
        }
        public async Task<float> DiemTrungBinhChungCacMonHocKy(int studentId, int classId, int semesterId, int schoolYearId)
        {
            float tongDiemCacMon = 0;
            float diemTBTungMon = 0;
            int tongHeSoCacMon = 0;

            var subjects = await assignmentRepository.GetSubject(schoolYearId, classId);
            if (subjects.Any())
            {
                foreach (Subject s in subjects)
                {
                    diemTBTungMon = await DiemTrungBinhMonHocKy(studentId, s.Id, semesterId, schoolYearId, classId);
                    tongDiemCacMon += diemTBTungMon * s.Factor;
                    tongHeSoCacMon += s.Factor;

                }
            }

            if (tongHeSoCacMon > 0)
                return tongDiemCacMon / tongHeSoCacMon;
            else
                return 0;
        }
        public async Task<float> DiemTrungBinhChungCacMonCaNam(int studentId, int classId, int schoolYearId)
        {
            float tongDiemCacMon = 0;
            float diemTBTungMon = 0;
            int tongHeSoCacMon = 0;

            var subjects = await assignmentRepository.GetSubject(schoolYearId, classId);
            if (subjects.Any())
            {
                foreach (Subject s in subjects)
                {
                    diemTBTungMon = await DiemTrungBinhMonCaNam(studentId, s.Id, schoolYearId, classId);
                    tongDiemCacMon += diemTBTungMon * s.Factor;
                    tongHeSoCacMon += s.Factor;
                }
            }

            if (tongHeSoCacMon > 0)
                return tongDiemCacMon / tongHeSoCacMon;
            else
                return 0;
        }
        public async Task<float> DiemTrungBinhMonCaNam(int studentId, int maMonHoc, int schoolYearId, int classId)
        {
            float tongDiemCacMon = 0;
            float diemTBTungMon = 0;
            int tongHeSoCacMon = 0;

            var subjects = await semesterRepository.GetAll();
            if (subjects.Any())
            {
                foreach (Semester s in subjects)
                {
                    diemTBTungMon = await DiemTrungBinhMonHocKy(studentId, maMonHoc, s.Id, schoolYearId, classId);
                    tongDiemCacMon += diemTBTungMon * s.Factor;
                    tongHeSoCacMon += s.Factor;


                }
            }

            if (tongHeSoCacMon > 0)
                return tongDiemCacMon / tongHeSoCacMon;
            else
                return 0;
        }
        #endregion
    }
}
