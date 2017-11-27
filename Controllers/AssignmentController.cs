using exam.Models;
using exam.Repository;
using exam.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models;
using StudentManager.Models.Post;
using StudentManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace exam.Controllers
{
    [Route("api/assignments")]
    public class AssignmentController : Controller
    {
        AssignmentRepository AssignmentRepository;
        SchoolYearRepository SchoolYearRepository;
        ClassRepository ClassRepository;
        SubjectRepository SubjectRepository;
        TeacherRepository TeacherRepository;

        public AssignmentController(AssignmentRepository assignmentRepository, SchoolYearRepository schoolYearRepository, ClassRepository classRepository, SubjectRepository subjectRepository, TeacherRepository teacherRepository)
        {
            AssignmentRepository = assignmentRepository;
            SchoolYearRepository = schoolYearRepository;
            ClassRepository = classRepository;
            SubjectRepository = subjectRepository;
            TeacherRepository = teacherRepository;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var nations = await AssignmentRepository.GetAll();
            if (nations == null || !nations.Any()) return NotFound(new { message = "Không có phân công nào" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = nations });
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var nation = await AssignmentRepository.Get(id);
            if (nation == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy phân công" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = nation });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] PostAssignment fromBody)
        {
            if (fromBody.SchoolYearId == default(int))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message="Thiếu mã năm học"
                });

            if (fromBody.SchoolYearId == default(int))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Thiếu mã lớp học"
                });

            if (fromBody.SchoolYearId == default(int))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Thiếu mã môn học"
                });

            if (fromBody.SchoolYearId == default(int))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Thiếu mã giáo viên học"
                });

            Assignment ass = new Assignment {
                SchoolYear = await SchoolYearRepository.Get(fromBody.SchoolYearId),
                Class = await ClassRepository.Get(fromBody.ClassId),
                Subject = await SubjectRepository.Get(fromBody.SubjectId),
                Teacher = await TeacherRepository.Get(fromBody.TeacherId)
            };
            await AssignmentRepository.Create(ass);
            return Ok(new
            {
                status = ResultStatus.STATUS_OK,
                data = ass
            });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, [FromBody] PostAssignment fromBody)
        {
            var exist = await AssignmentRepository.Get(id);
            if (exist == null) return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy phân công" });


            if (fromBody.SchoolYearId == default(int))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Thiếu mã năm học"
                });

            if (fromBody.SchoolYearId == default(int))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Thiếu mã lớp học"
                });

            if (fromBody.SchoolYearId == default(int))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Thiếu mã môn học"
                });

            if (fromBody.SchoolYearId == default(int))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Thiếu mã giáo viên học"
                });


            exist.SchoolYear = await SchoolYearRepository.Get(fromBody.SchoolYearId);
            exist.Class = await ClassRepository.Get(fromBody.ClassId);
            exist.Subject = await SubjectRepository.Get(fromBody.SubjectId);
            exist.Teacher = await TeacherRepository.Get(fromBody.TeacherId);

            await AssignmentRepository.Update(id, exist);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Sửa thông tin phân công thành công", data = exist });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exist = await AssignmentRepository.Get(id);
            if (exist == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy phân công" });
            await AssignmentRepository.Delete(id);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Xóa thành công!" });
        }
    }
}

