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

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var subjects = await subjectRepository.getAll();
            if (subjects == null || !subjects.Any()) return NotFound(new { message = "Không có môn học nào" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = subjects });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchoolYear(int id)
        {
            var subject = await subjectRepository.Get(id);
            if (subject == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy môn học" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = subject });
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] Subject subject)
        {
            if (String.IsNullOrEmpty(subject.Name))
                return Ok(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Tên môn học không được để trống"
                });
            if (subject.LessionSize == default(int))
                return Ok(new
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

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] Subject subject)
        {
            var exist = await subjectRepository.Get(subject.Id);
            if (exist == null) return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy môn học" });

            if (String.IsNullOrEmpty(subject.Name))
                return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên môn học không được để trống" });


            exist.Name = subject.Name;
            exist.LessionSize = subject.LessionSize;
            exist.Factor = subject.Factor;

            await subjectRepository.Update(subject.Id, subject);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Sửa thông tin môn học thành công", data = subject });
        }

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
