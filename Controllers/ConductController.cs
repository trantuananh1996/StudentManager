using exam.Models;
using exam.Repository;
using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Controllers
{
    [Route("api/conducts")]
    public class ConductController : Controller
    {
        ConductRepository ConductRepository;

        public ConductController(ConductRepository repo)
        {
            this.ConductRepository = repo;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var nations = await ConductRepository.GetAll();
            if (nations == null || !nations.Any()) return NotFound(new { message = "Không có hạnh kiểm nào" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = nations });
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var nation = await ConductRepository.Get(id);
            if (nation == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy hạnh kiểm" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = nation });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] Conduct fromBody)
        {
            if (String.IsNullOrEmpty(fromBody.Name))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Tên hạnh kiểm không được để trống"
                });
            var exist = await ConductRepository.FindByName(fromBody.Name);
            if (exist != null) return BadRequest(new
            {
                status = ResultStatus.STATUS_DUPLICATE,
                message = "Đã có hạnh kiểm này tồn tại trong hệ thống"
            });
            await ConductRepository.Create(fromBody);
            return Ok(new
            {
                status = ResultStatus.STATUS_OK,
                data = fromBody
            });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id,[FromBody] Conduct fromBody)
        {
            var exist = await ConductRepository.Get(id);
            if (exist == null) return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy hạnh kiểm" });

            if (String.IsNullOrEmpty(fromBody.Name))
                return BadRequest(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên hạnh kiểm không được để trống" });


            exist.Name = fromBody.Name;

            await ConductRepository.Update(id, fromBody);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Sửa thông tin hạnh kiểm thành công", data = exist });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exist = await ConductRepository.Get(id);
            if (exist == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy hạnh kiểm" });
            await ConductRepository.Delete(id);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Xóa thành công!" });
        }
    }
}
