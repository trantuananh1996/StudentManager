using exam.Models;
using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Controllers
{
    [Route("api/subject")]
    public class SubjectController : Controller
    {
        SubjectRepository subjectRepository;

        public SubjectController(SubjectRepository subjectRepository)
        {
            this.subjectRepository = subjectRepository;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var subjects = await subjectRepository.GetAll();
            if (subjects == null || !subjects.Any()) return NotFound(new { message = "Không có môn học nào" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = subjects });
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var subject = await subjectRepository.Get(id);
            if (subject == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy môn học" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = subject });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Subject subject)
        {
            if (String.IsNullOrEmpty(subject.Name))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Tên môn học không được để trống"
                });
            if (subject.LessionSize == default(int))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Số tiết học không được để trống"
                });
            if (subject.Factor == default(int)) subject.Factor = 1;

            await subjectRepository.Create(subject);
            return Ok(new
            {
                status = ResultStatus.STATUS_OK,
                data = subject
            });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id,[FromBody] Subject subject)
        {
            var exist = await subjectRepository.Get(id);
            if (exist == null) return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy môn học" });

            if (String.IsNullOrEmpty(subject.Name))
                return BadRequest(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên môn học không được để trống" });
            if (subject.LessionSize == default(int))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Số tiết học không được để trống"
                });

            exist.Name = subject.Name;
            exist.LessionSize = subject.LessionSize;
            exist.Factor = subject.Factor;

            await subjectRepository.Update(id, subject);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Sửa thông tin môn học thành công", data = exist });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await subjectRepository.Get(id);
            if (user == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy môn học" });
            await subjectRepository.Delete(id);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Xóa thành công!" });
        }

    }
}
