using exam.Models;
using exam.Repository;
using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models;
using StudentManager.Models.Point;
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
        AssignmentRepository assignmentRepository;

        public PointController(PointTypeRepository pointTypeRepository, PointRepository pointRepository, RuleRepository ruleRepository, StudentRepository studentRepository, SubjectRepository subjectRepository, SemesterRepository semesterRepository, SchoolYearRepository schoolYearRepository, ClassRepository classRepository, AssignmentRepository assignmentRepository)
        {
            this.pointTypeRepository = pointTypeRepository;
            this.pointRepository = pointRepository;
            this.ruleRepository = ruleRepository;
            this.studentRepository = studentRepository;
            this.subjectRepository = subjectRepository;
            this.semesterRepository = semesterRepository;
            this.schoolYearRepository = schoolYearRepository;
            this.classRepository = classRepository;
            this.assignmentRepository = assignmentRepository;
        }

        [HttpGet("point-types")]
        public async Task<ActionResult> GetPointTypes()
        {
            List<PointType> pointtypes = await pointTypeRepository.GetAll();
            if (pointtypes == null || pointtypes.Count() == 0)
                return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không có loại điểm nào" });
            else
                return Ok(new { status = ResultStatus.STATUS_OK, data = pointtypes });
        }

        private bool IsPointInvalid(float point, float maxPoint)
        {
            if (maxPoint == default(float)) maxPoint = 10;
            if (point < 0 || point > maxPoint) return true;
            else return false;
        }

        [HttpPost("update-point")]
        public async Task<ActionResult> CreateOrUpdate([FromBody] PostUpdatePoint givenPoint)
        {
            if (PointController.maxPoint == default(float))
            {
                Rule rule = await ruleRepository.GetDefaultRule();
                maxPoint = rule.MaxPoint == default(float) ? 0 : rule.MaxPoint;
            }

            if (IsPointInvalid(givenPoint.PointNumber, maxPoint))
            {
                return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Số điểm không hợp lệ" });
            }
            else
            if (givenPoint.Id == default(int))
            {
                Point point = new Point
                {
                    Student = await studentRepository.Get(givenPoint.StudentId),
                    Subject = await subjectRepository.Get(givenPoint.SubjectId),
                    Semester = await semesterRepository.Get(givenPoint.SemesterId),
                    SchoolYear = await schoolYearRepository.Get(givenPoint.SchoolYearId),
                    Class = await classRepository.Get(givenPoint.ClassId),
                    PointType = await pointTypeRepository.Get(givenPoint.PointTypeId),
                    PointNumber = givenPoint.PointNumber
                };
                await pointRepository.Create(point);
                return Ok(new { status = ResultStatus.STATUS_OK, data = point });
            }
            else
            {
                var existPoint = await pointRepository.Get(givenPoint.Id);
                if (existPoint == null)
                {
                    Point point = new Point
                    {
                        Student = await studentRepository.Get(givenPoint.StudentId),
                        Subject = await subjectRepository.Get(givenPoint.SubjectId),
                        Semester = await semesterRepository.Get(givenPoint.SemesterId),
                        SchoolYear = await schoolYearRepository.Get(givenPoint.SchoolYearId),
                        Class = await classRepository.Get(givenPoint.ClassId),
                        PointType = await pointTypeRepository.Get(givenPoint.PointTypeId),
                        PointNumber = givenPoint.PointNumber
                    };
                    await pointRepository.Create(point);
                    return Ok(new
                    {
                        status = ResultStatus.STATUS_OK,
                        data = givenPoint
                    });
                }
                else
                {
                    existPoint.Student = await studentRepository.Get(givenPoint.StudentId);
                    existPoint.Subject = await subjectRepository.Get(givenPoint.SubjectId);
                    existPoint.Semester = await semesterRepository.Get(givenPoint.SemesterId);
                    existPoint.SchoolYear = await schoolYearRepository.Get(givenPoint.SchoolYearId);
                    existPoint.Class = await classRepository.Get(givenPoint.ClassId);
                    existPoint.PointType = await pointTypeRepository.Get(givenPoint.PointTypeId);
                    existPoint.PointNumber = givenPoint.PointNumber;
                    return Ok(new { status = ResultStatus.STATUS_OK, data = givenPoint });
                }
            }
        }
        [HttpPost("update-point")]
        public async Task<ActionResult> CreateOrUpdate([FromBody] List<PostUpdatePoint> points)
        {
            foreach (PostUpdatePoint p in points)
            {
                await CreateOrUpdate(p);
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

        public async Task<float> DiemTrungBinhKiemTra(int maHocSinh, int maMonHoc, int maHocKy, int maNamHoc, int maLop)
        {
            var list = await pointRepository.GetStudentPoint(maHocSinh, maMonHoc, maHocKy, maNamHoc, maLop);
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

        public async Task<float> DiemTrungBinhMonHocKy(int maHocSinh, int maMonHoc, int maHocKy, int maNamHoc, int maLop)
        {
            var list = await pointRepository.GetStudentPoint(maHocSinh, maMonHoc, maHocKy, maNamHoc, maLop);
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
        public async Task<float> DiemTrungBinhChungCacMonHocKy(int maHocSinh, int maLop, int maHocKy, int maNamHoc)
        {
            float tongDiemCacMon = 0;
            float diemTBTungMon = 0;
            int tongHeSoCacMon = 0;

            var subjects = await assignmentRepository.GetSubject(maNamHoc, maLop);
            if (subjects.Any())
            {
                foreach(Subject s in subjects)
                {
                    diemTBTungMon = await DiemTrungBinhMonHocKy(maHocSinh, s.Id,maHocKy, maNamHoc, maLop);
                    tongDiemCacMon += diemTBTungMon * s.Factor;
                    tongHeSoCacMon += s.Factor;

                }
            }
           
            if (tongHeSoCacMon > 0)
                return tongDiemCacMon / tongHeSoCacMon;
            else
                return 0;
        }

        public async Task<float> DiemTrungBinhChungCacMonCaNam(int maHocSinh, int maLop, int maNamHoc)
        {
            float tongDiemCacMon = 0;
            float diemTBTungMon = 0;
            int tongHeSoCacMon = 0;

            var subjects = await assignmentRepository.GetSubject(maNamHoc, maLop);
            if (subjects.Any())
            {
                foreach (Subject s in subjects)
                {
                    diemTBTungMon = await DiemTrungBinhMonCaNam(maHocSinh, s.Id, maNamHoc, maLop);
                    tongDiemCacMon += diemTBTungMon *s.Factor;
                    tongHeSoCacMon += s.Factor;
                }
            }

            if (tongHeSoCacMon > 0)
                return tongDiemCacMon / tongHeSoCacMon;
            else
                return 0;
        }
        public async Task<float> DiemTrungBinhMonCaNam(int maHocSinh, int maMonHoc, int maNamHoc, int maLop)
        {
            float tongDiemCacMon = 0;
            float diemTBTungMon = 0;
            int tongHeSoCacMon = 0;

            var subjects = await semesterRepository.GetAll();
            if (subjects.Any())
            {
                foreach (Semester s in subjects)
                {
                    diemTBTungMon = await DiemTrungBinhMonHocKy(maHocSinh,maMonHoc, s.Id, maNamHoc, maLop);
                    tongDiemCacMon += diemTBTungMon * s.Factor;
                    tongHeSoCacMon +=s.Factor;


                }
            }

            if (tongHeSoCacMon > 0)
                return tongDiemCacMon / tongHeSoCacMon;
            else
                return 0;
        }
    }
}
