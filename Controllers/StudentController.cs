using exam.Models;
using exam.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam.Utils;
using Microsoft.AspNetCore.Authorization;
using exam.Models.Post;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using StudentManager.Repository;
using StudentManager.Models;

namespace exam.Controllers
{
    [Route("api/students")]
    public class StudentController : Controller
    {
        StudentRepository studentRepository;
        NationRepository nationRepository;
        ClassRepository classRepo;
        ReligionRepository religionRepository;
        JobRepository jobRepository;
        RuleRepository ruleRepo;

        public StudentController(StudentRepository studentRepository, NationRepository nationRepository, ClassRepository classRepo, ReligionRepository religionRepository, JobRepository jobRepository, RuleRepository ruleRepo)
        {
            this.studentRepository = studentRepository;
            this.nationRepository = nationRepository;
            this.classRepo = classRepo;
            this.religionRepository = religionRepository;
            this.jobRepository = jobRepository;
            this.ruleRepo = ruleRepo;
        }

        [Authorize(Roles = "SchoolStaff,SchoolBoard")]
        [HttpGet]
        public async Task<IActionResult> PostAsync()
        {
            List<Student> lstItems = await studentRepository.GetAll();
            dynamic response = new
            {
                Data = lstItems,
            };
            return Ok(response);
        }
        [Authorize(Roles = "SchoolStaff,SchoolBoard,Teacher")]
        [HttpGet("find")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<ActionResult> FindStudentByName(string name)
        {
            List<Student> students = await studentRepository.FindStudentByName(name);
            if (students == null || students.Count() == 0)
                return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });
            else return Ok(new { status = ResultStatus.STATUS_OK, data = students });
        }

        [HttpGet("class/{classId}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard,SchoolStaff")]
        public async Task<IActionResult> List(int classId)
        {
            var clazz = await classRepo.Get(classId);
            var users = await studentRepository.FindStudentByClass(classId);
            return Ok(new { status = ResultStatus.STATUS_OK, data = new { classInfo = clazz, students = users } });

        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard,SchoolStaff,Teacher")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await studentRepository.Get(id);
            if (user == null) return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = user });
        }

        private int CalcAge(DateTime birth)
        {
            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - birth.Year;
            // Go back to the year the person was born in case of a leap year
            if (birth > today.AddYears(-age)) age--;
            return age;
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolStaff,SchoolBoard")]
        public async Task<IActionResult> Create([FromBody] Student u)
        {
            if (String.IsNullOrEmpty(u.FullName)) return BadRequest(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên học sinh không được để trống" });
            Rule rule = await ruleRepo.GetDefaultRule();
            if (rule != null && u.BirthDay != null)
            {
                int age = CalcAge(u.BirthDay);
                if (age < rule.MinAge) return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT
                  ,
                    message = "Tuổi học sinh quá nhỏ"
                });
                if (age > rule.MaxAge) return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT
                 ,
                    message = "Tuổi học sinh quá lớn"
                });
            }
            if (u.Nation != null)
            {
                var eNation = await nationRepository.FindByName(u.Nation.Name);
                if (eNation == null) await nationRepository.Create(u.Nation);
            }
            if (u.Religion != null)
            {
                var eReligion = await religionRepository.FindByName(u.Religion.Name);
                if (eReligion == null) await religionRepository.Create(u.Religion);
            }
            if (u.FatherJob != null)
            {
                var eJob = await jobRepository.FindByName(u.FatherJob.Name);
                if (eJob == null) await jobRepository.Create(u.FatherJob);
            }
            if (u.MotherJob != null)
            {
                var eJob = await jobRepository.FindByName(u.MotherJob.Name);
                if (eJob == null) await jobRepository.Create(u.MotherJob);
            }

            return Ok(new
            {
                status = ResultStatus.STATUS_OK,
                data = await studentRepository.Create(u)
            });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard,SchoolStaff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Student student)
        {
            var user = await studentRepository.Get(id);
            if (user == null) return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });

            if (String.IsNullOrEmpty(student.FullName)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên học sinh không được để trống" });

            user.FullName = student.FullName;
            await studentRepository.Update(id, user);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Sửa thông tin học sinh thành công", data = student });
        }

        [HttpDelete("{id}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard,SchoolStaff")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await studentRepository.Get(id);
            if (user == null) return BadRequest(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });
            await studentRepository.Delete(id);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Xóa thành công!" });
        }




    }
}
