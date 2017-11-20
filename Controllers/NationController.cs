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
    [Route("api/nations")]
    public class NationController : Controller
    {
        NationRepository NationRepository;

        public NationController(NationRepository repo)
        {
            this.NationRepository = repo;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var nations = await NationRepository.GetAll();
            if (nations == null || !nations.Any()) return NotFound(new { message = "Không có dân tộc nào" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = nations });
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var nation = await NationRepository.Get(id);
            if (nation == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy dân tộc" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = nation });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] Nation fromBody)
        {
            if (String.IsNullOrEmpty(fromBody.Name))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Tên dân tộc không được để trống"
                });
            var exist = await NationRepository.FindByName(fromBody.Name);
            if (exist != null) return BadRequest(new
            {
                status = ResultStatus.STATUS_DUPLICATE,
                message = "Đã có dân tộc này tồn tại trong hệ thống"
            });
            await NationRepository.Create(fromBody);
            return Ok(new
            {
                status = ResultStatus.STATUS_OK,
                data = fromBody
            });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, [FromBody] Nation fromBody)
        {
            var exist = await NationRepository.Get(id);
            if (exist == null) return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy dân tộc" });

            if (String.IsNullOrEmpty(fromBody.Name))
                return BadRequest(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên dân tộc không được để trống" });


            exist.Name = fromBody.Name;

            await NationRepository.Update(id, fromBody);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Sửa thông tin dân tộc thành công", data = exist });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exist = await NationRepository.Get(id);
            if (exist == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy dân tộc" });
            await NationRepository.Delete(id);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Xóa thành công!" });
        }
    }
}

