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
    [Route("api/grades")]
    public class GradeController : Controller
    {
        GradeRepository GradeRepository;

        public GradeController(GradeRepository repo)
        {
            this.GradeRepository = repo;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var nations = await GradeRepository.GetAll();
            if (nations == null || !nations.Any()) return NotFound(new { message = "Không có khối lớp nào" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = nations });
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var nation = await GradeRepository.Get(id);
            if (nation == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy khối lớp" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = nation });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] Grade fromBody)
        {
            if (String.IsNullOrEmpty(fromBody.Name))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Tên khối lớp không được để trống"
                });
            var exist = await GradeRepository.FindByName(fromBody.Name);
            if (exist != null) return BadRequest(new
            {
                status = ResultStatus.STATUS_DUPLICATE,
                message = "Đã có khối lớp này tồn tại trong hệ thống"
            });
            await GradeRepository.Create(fromBody);
            return Ok(new
            {
                status = ResultStatus.STATUS_OK,
                data = fromBody
            });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, [FromBody] Grade fromBody)
        {
            var exist = await GradeRepository.Get(id);
            if (exist == null) return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy khối lớp" });

            if (String.IsNullOrEmpty(fromBody.Name))
                return BadRequest(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên khối lớp không được để trống" });


            exist.Name = fromBody.Name;

            await GradeRepository.Update(id, fromBody);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Sửa thông tin khối lớp thành công", data = exist });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exist = await GradeRepository.Get(id);
            if (exist == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy khối lớp" });
            await GradeRepository.Delete(id);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Xóa thành công!" });
        }
    }
}

