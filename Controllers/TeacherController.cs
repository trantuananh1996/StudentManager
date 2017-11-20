using exam.Models;
using exam.Repository;
using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Controllers
{

    [Route("api/teachers")]
    public class TeacherController : Controller
    {
        TeacherRepository teacherRepository;
        SubjectRepository subjectRepository;

        public TeacherController(TeacherRepository teacherRepository, SubjectRepository subjectRepository)
        {
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var list = await teacherRepository.GetAll();
            if (list == null || !list.Any()) return NotFound(new { message = "Không có giáo viên nào" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = list });
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacher(int id)
        {
            var item = await teacherRepository.Get(id);
            if (item == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy giáo viên" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = item });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Teacher teacher)
        {
            if (String.IsNullOrEmpty(teacher.Name))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Tên giáo viên không được để trống"
                });
            if (String.IsNullOrEmpty(teacher.Phone))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Số điện thoại giáo viên không được để trống"
                });
            if (String.IsNullOrEmpty(teacher.Address))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Địa chỉ giáo viên không được để trống"
                });

            await teacherRepository.Create(teacher);
            return Ok(new
            {
                status = ResultStatus.STATUS_OK,
                data = teacher
            });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] Teacher teacher)
        {
            var exist = await teacherRepository.Get(teacher.Id);
            if (exist == null) return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy giáo viên" });

            if (String.IsNullOrEmpty(teacher.Name))
                return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên giáo viên không được để trống" });
            if (String.IsNullOrEmpty(teacher.Phone))
                return Ok(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Số điện thoại giáo viên không được để trống"
                });
            if (String.IsNullOrEmpty(teacher.Address))
                return Ok(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Địa chỉ giáo viên không được để trống"
                });


            exist.Name = teacher.Name;
            exist.Address = teacher.Address;
            exist.Phone = teacher.Phone;
            exist.Subject = teacher.Subject;

            await teacherRepository.Update(teacher.Id, teacher);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Sửa thông tin giáo viên thành công", data = teacher });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await teacherRepository.Get(id);
            if (teacher == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy giáo viên" });
            await teacherRepository.Delete(id);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Xóa thành công!" });
        }
    }
}
