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

namespace exam.Controllers
{
    [Route("api/students")]
    public class StudentController : Controller
    {
        IStudentRepository studentRepository;
        INationRepository nationRepository;
        IReligionRepository religionRepository;
        public StudentController(IStudentRepository user,INationRepository nat,IReligionRepository rel)
        {
            studentRepository = user;
            nationRepository = nat;
            religionRepository = rel;
        }

        [HttpGet("find")]
        public async Task<ActionResult> FindStudentByName(string name)
        {
            List<Student> students= await studentRepository.FindStudentByName(name);
            if (students == null || students.Count() == 0)
                return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });
            else return Ok(new { status = ResultStatus.STATUS_OK, data = students });
        }

        [HttpPost("list")]
        public async Task<IActionResult> List([FromBody] PostClassId Class)
        {
            if (Class.ClassId==0)
            {
                var users = await studentRepository.getAll();
                return Ok(new {status = ResultStatus.STATUS_OK, data= users });
            }
            else
            {
                var users = await studentRepository.FindStudentByClass(Class.ClassId);
                return Ok(new { status = ResultStatus.STATUS_OK, data = users });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await studentRepository.Get(id);
            if (user == null) return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });
            return Ok(new { status=ResultStatus.STATUS_OK,data=user });
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] Student u)
        {
            if (String.IsNullOrEmpty(u.FullName)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message="Tên học sinh không được để trống"});

            if (u.Nation!=null)  await nationRepository.Create(u.Nation);
            if (u.Religion!=null) await religionRepository.Create(u.Religion);
            await studentRepository.Create(u);
            return Ok(new
            {  
                status=ResultStatus.STATUS_OK,
                data = u
            });
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit( [FromBody] Student student)
        {
            var user = await studentRepository.Get(student.Id);
            if (user == null) return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });

            if (String.IsNullOrEmpty(student.FullName)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên học sinh không được để trống" });
            
            await studentRepository.Create(student);
            return Ok(new { status=ResultStatus.STATUS_OK,message = "Sửa thông tin học sinh thành công", data = student });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await studentRepository.Get(id);
            if (user == null) return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });
            await studentRepository.Delete(id);
            return Ok(new { status =ResultStatus.STATUS_OK,message = "Xóa thành công!" });
        }




    }
}
