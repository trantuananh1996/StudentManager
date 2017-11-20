using exam.Models;
using exam.Repository;
using exam.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models;
using StudentManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace exam.Controllers
{
    [Route("api/grade")]
    public class GradeController : Controller
    {
        GradeRepository gradeRepository;

        public GradeController(GradeRepository gradeRepository)
        {
            this.gradeRepository = gradeRepository;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var grade = await gradeRepository.getAll();
            if (grade == null || !grade.Any()) return NotFound(new { message = "Du lieu khong ton tai" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = grade });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchoolYear(int id)
        {
            var grade = await gradeRepository.Get(id);
            if (grade == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy nghề nghiệp" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = grade });
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] Grade grade)
        {
            if (String.IsNullOrEmpty(grade.Name))
                return Ok(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Tên nghề nghiệp không được để trống"
                });
            await gradeRepository.Create(grade);
            return Ok(new
            {
                status = ResultStatus.STATUS_OK,
                data = grade
            });
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] Grade grade)
        {
            var user = await gradeRepository.Get(grade.Id);
            if (user == null) return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy nghề nghiệp" });

            if (String.IsNullOrEmpty(grade.Name))
                return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên khối không được để trống" });


            user.Name = grade.Name;
            await gradeRepository.Update(grade.Id, grade);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Sửa thông tin khối thanh cong", data = grade });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await gradeRepository.Get(id);
            if (user == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy khối" });
            await gradeRepository.Delete(id);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Xóa thành công!" });
        }

    }
}

