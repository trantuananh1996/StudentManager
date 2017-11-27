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

namespace exam.Controllers
{
    [Route("api/students")]
    public class StudentController : Controller
    {
        StudentRepository studentRepository;
        NationRepository nationRepository;
        ReligionRepository religionRepository;
        public StudentController(StudentRepository user, NationRepository nat,ReligionRepository rel)
        {
            studentRepository = user;
            nationRepository = nat;
            religionRepository = rel;
        }

        [Authorize(Roles ="SchoolStaff")]
        [HttpGet]
        public async Task<IActionResult> PostAsync()
        {
            //Get form data from client side
            List<Student> lstItems =  await studentRepository.GetAll();


            // Custom response to bind information in client side
            dynamic response = new
            {
                Data = lstItems,
            };
            return Ok(response);
        }
        [Authorize(Roles = "SchoolStaff")]
        [HttpGet("find")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<ActionResult> FindStudentByName(string name)
        {
            List<Student> students= await studentRepository.FindStudentByName(name);
            if (students == null || students.Count() == 0)
                return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });
            else return Ok(new { status = ResultStatus.STATUS_OK, data = students });
        }

        [HttpPost("{classId}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard,SchoolStaff")]
        public async Task<IActionResult> List(int classId)
        {
            if (classId == 0)
            {
                var users = await studentRepository.GetAll();
                return Ok(new {status = ResultStatus.STATUS_OK, data= users });
            }
            else
            {
                var users = await studentRepository.FindStudentByClass(classId);
                return Ok(new { status = ResultStatus.STATUS_OK, data = users });
            }
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard,SchoolStaff")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await studentRepository.Get(id);
            if (user == null) return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });
            return Ok(new { status=ResultStatus.STATUS_OK,data=user });
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolStaff,SchoolBoard")]
        public async Task<IActionResult> Create([FromBody] Student u)
        {
            if (String.IsNullOrEmpty(u.FullName)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message="Tên học sinh không được để trống"});

            if (u.Nation!=null)  await nationRepository.Create(u.Nation);
            if (u.Religion!=null) await religionRepository.Create(u.Religion);
            await studentRepository.Create(u);
            return Ok(new
            {  
                status=ResultStatus.STATUS_OK,
                data = await studentRepository.Get(u.Id)
            });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard,SchoolStaff")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit( int id,[FromBody] Student student)
        {
            var user = await studentRepository.Get(id);
            if (user == null) return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });

            if (String.IsNullOrEmpty(student.FullName)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên học sinh không được để trống" });
            
            await studentRepository.Update(id,student);
            return Ok(new { status=ResultStatus.STATUS_OK,message = "Sửa thông tin học sinh thành công", data = student });
        }

        [HttpDelete("{id}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard,SchoolStaff")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await studentRepository.Get(id);
            if (user == null) return BadRequest(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });
            await studentRepository.Delete(id);
            return Ok(new { status =ResultStatus.STATUS_OK,message = "Xóa thành công!" });
        }




    }
}
