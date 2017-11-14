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
    [Route("api/schoolYear")]
    public class SchoolYearController : Controller
    {
        SchoolYearRepository schoolYearRepository;

        public SchoolYearController(SchoolYearRepository schoolYearRepository)
        {
            this.schoolYearRepository = schoolYearRepository;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var SchoolYears = await schoolYearRepository.getAll();
            if (SchoolYears == null || !SchoolYears.Any()) return NotFound(new { message = "Không có năm học nào" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = SchoolYears });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchoolYear(int id)
        {
            var schoolyear = await schoolYearRepository.Get(id);
            if (schoolyear == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy năm học" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = schoolyear });
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] SchoolYear schoolyear)
        {
            if (String.IsNullOrEmpty(schoolyear.Name))
                return Ok(new {
                    status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên năm học không được để trống" });
            await schoolYearRepository.Create(schoolyear);
            return Ok(new
            {
                status = ResultStatus.STATUS_OK,
                data = schoolyear
            });
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] SchoolYear student)
        {
            var user = await schoolYearRepository.Get(student.Id);
            if (user == null) return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy năm học" });

            if (String.IsNullOrEmpty(student.Name))
                return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên năm học không được để trống" });


            user.Name = student.Name;
            await schoolYearRepository.Update(student.Id,student);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Sửa thông tin năm học thành công", data = student });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await schoolYearRepository.Get(id);
            if (user == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy năm học" });
            await schoolYearRepository.Delete(id);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Xóa thành công!" });
        }

    }
}
