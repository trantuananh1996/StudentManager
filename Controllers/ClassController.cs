using exam.Models;
using exam.Repository;
using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models;
using StudentManager.Models.Point;
using StudentManager.Models.Post;
using StudentManager.Repository;
using StudentManager.Repository.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Controllers
{
    [Route("api/classes")]
    public class ClassController : Controller
    {
        ClassRepository classRepository;
        GradeRepository GradeRepository;
        SchoolYearRepository SchoolYearRepository;
        TeacherRepository TeacherRepository;
        RuleRepository ruleRepo;

        public ClassController(ClassRepository classRepository, GradeRepository gradeRepository, SchoolYearRepository schoolYearRepository, TeacherRepository teacherRepository, RuleRepository ruleRepo)
        {
            this.classRepository = classRepository;
            GradeRepository = gradeRepository;
            SchoolYearRepository = schoolYearRepository;
            TeacherRepository = teacherRepository;
            this.ruleRepo = ruleRepo;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var list = await classRepository.GetAll();
            if (list == null || !list.Any()) return NotFound(new { message = "Không có lớp nào" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = list });
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await classRepository.Get(id);
            if (item == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy lớp" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = item });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPost]
        public async  Task<IActionResult> Create([FromBody] PostCreateClass fromBody)
        {
            if (String.IsNullOrEmpty(fromBody.Name))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Tên lớp không được để trống"
                });
            if (fromBody.Size == default(int))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Số lương học sinh trong lớp không được để trống"
                });
            Rule rule = await ruleRepo.GetDefaultRule();
            if (rule != null)
            {
                if (fromBody.Size > rule.MaxSize) return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Số lương học sinh tối đa trong lớp vượt quá quy định (" + rule.MaxSize + ") học sinh"
                });
                if (fromBody.Size <rule.MinSize) return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Số lương học sinh tối thiểu trong lớp thấp quá quy định (" + rule.MinSize + ") học sinh"
                });
            }
            Class cls = new Class
            {
                Name = fromBody.Name,
                Grade = await GradeRepository.Get(fromBody.GradeId),
                SchoolYear = await SchoolYearRepository.Get(fromBody.SchoolYearId),
                Teacher = await TeacherRepository.Get(fromBody.TeacherId),
                Size = fromBody.Size
            };
            await classRepository.Create(cls);
            return Ok(new
            {
                status = ResultStatus.STATUS_OK,
                data = cls
            });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, [FromBody] PostCreateClass fromBody)
        {
            var exist = await classRepository.Get(id);
            if (exist == null) return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy lớp" });

            if (String.IsNullOrEmpty(fromBody.Name))
                return BadRequest(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên lớp không được để trống" });


            exist.Name = fromBody.Name;
            exist.Grade = await GradeRepository.Get(fromBody.GradeId);
            exist.SchoolYear = await SchoolYearRepository.Get(fromBody.SchoolYearId);
            exist.Teacher = await TeacherRepository.Get(fromBody.TeacherId);
            exist.Size = fromBody.Size;

            await classRepository.Update(id, exist);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Sửa thông tin lớp thành công", data = exist });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await classRepository.Get(id);
            if (item == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy lớp" });
            await classRepository.Delete(id);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Xóa thành công!" });
        }
    }
}
