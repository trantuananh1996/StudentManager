using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using exam.Repository;
using exam.Utils;
using exam.Models.Post;

namespace exam.Controllers
{
    [Route("api/studentClasses")]
    public class StudentClassController : Controller
    {
        IClassRepository classRepository;
        IStudentClassRepository studentClassRepository;
        public StudentClassController(IClassRepository classRepository, IStudentClassRepository stC)
        {
            this.classRepository = classRepository;
            this.studentClassRepository = stC;
        }

        [HttpGet("classes")]
        public async Task<IActionResult> List()
        {
            var classes = await classRepository.getAll();
            return Ok(new { status = ResultStatus.STATUS_OK, data = classes });
        }

        [HttpPost("createClass")]
        public async Task<IActionResult> createClass([FromBody] PostAddStudentClass post)
        {
            if (post.ClassId == 0) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Thiếu classId" });
            if (!post.StudentIds.Any()) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Thiếu danh sách học sinh" });

            foreach(int sId in post.StudentIds)
            {
                await studentClassRepository.Create(new Models.StudentClass {StudentId=sId,ClassId=post.ClassId });
            }
            return Ok(new {status=ResultStatus.STATUS_OK,data=new { classId=post.ClassId,students=post.StudentIds} });
        }
    }
}