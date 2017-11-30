using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using exam.Repository;
using exam.Utils;
using exam.Models.Post;
using StudentManager.Models.Post;
using exam.Models;
using StudentManager.Repository;
using StudentManager.Models;

namespace exam.Controllers
{
    [Route("api/studentClasses")]
    public class StudentClassController : Controller
    {
        ClassRepository classRepository;
        StudentClassRepository studentClassRepository;
        StudentRepository studentRepository;
        RuleRepository ruleRepo;

        public StudentClassController(ClassRepository classRepository, StudentClassRepository studentClassRepository, StudentRepository studentRepository, RuleRepository ruleRepo)
        {
            this.classRepository = classRepository;
            this.studentClassRepository = studentClassRepository;
            this.studentRepository = studentRepository;
            this.ruleRepo = ruleRepo;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var classes = await studentClassRepository.GetAssignedClassAsync();
            List<dynamic> ls = new List<dynamic>();
            foreach (Class cls in classes)
            {
                var students = await studentRepository.FindStudentByClass(cls.Id);
                ls.Add(new { classInfo = cls, students = students });
            }

            return Ok(new { status = ResultStatus.STATUS_OK, data = ls });
        }
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPost]
        public async Task<IActionResult> CreateClass([FromBody] PostAddStudentClass post)
        {
            if (post.ClassId == default(int)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Thiếu classId" });
            if (post.StudentIds == null || !post.StudentIds.Any()) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Thiếu danh sách học sinh" });
            var clazz = await classRepository.Get(post.ClassId);
            var students = await studentRepository.FindStudentByClass(post.ClassId);
            if (students != null && students.Count() == clazz.Size)
            {
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Lớp đã đủ học sinh"
                });
            }
            else
            {
                int remain = clazz.Size - (students == null ? 0 : students.Count);
                if (remain < post.StudentIds.Count) return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Lớp chỉ còn chỗ cho " + remain + " học sinh"
                });
            }

            foreach (int sId in post.StudentIds)
            {
                await studentClassRepository.Create(new Models.StudentClass { StudentId = sId, ClassId = post.ClassId });
            }
            return Ok(new
            {
                status = ResultStatus.STATUS_OK,
                data = new
                {
                    classInfo = clazz,
                    students = await studentRepository.FindStudentByClass(post.ClassId)
                }
            });
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