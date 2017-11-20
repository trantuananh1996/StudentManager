using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using exam.Repository;
using exam.Utils;
using exam.Models.Post;
using StudentManager.Models.Post;

namespace exam.Controllers
{
    [Route("api/studentClasses")]
    public class StudentClassController : Controller
    {
        ClassRepository classRepository;
        StudentClassRepository studentClassRepository;
        public StudentClassController(ClassRepository classRepository, StudentClassRepository stC)
        {
            this.classRepository = classRepository;
            this.studentClassRepository = stC;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("classes")]
        public async Task<IActionResult> List()
        {
            var classes = await classRepository.getAll();
            return Ok(new { status = ResultStatus.STATUS_OK, data = classes });
        }
        [Microsoft.AspNetCore.Authorization.Authorize(Roles="SchoolBoard")]
        [HttpPost("createClass")]
        public async Task<IActionResult> CreateClass([FromBody] PostAddStudentClass post)
        {
            if (post.ClassId == default(int)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Thiếu classId" });
            if (!post.StudentIds.Any()) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Thiếu danh sách học sinh" });

            foreach(int sId in post.StudentIds)
            {
                await studentClassRepository.Create(new Models.StudentClass {StudentId=sId,ClassId=post.ClassId });
            }
            return Ok(new {status=ResultStatus.STATUS_OK,data=new { classId=post.ClassId,students=post.StudentIds} });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPost("move-class")]
        public async Task<ActionResult> MoveClass([FromBody] PostMoveClass post)
        {
            if (post.SourceClass == default(int)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Thiếu mã lớp cũ" });
            if (post.TargetClass == default(int)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Thiếu mã lớp mới" });
            if (!post.StudentIds.Any()) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Thiếu danh sách học sinh" });
            await studentClassRepository.MoveClass(post);
            return Ok(new { status = ResultStatus.STATUS_OK, data = new { classId = post.TargetClass, students = post.StudentIds } });

        }
    }
}