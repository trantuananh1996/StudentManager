using exam.Models;
using exam.Repository;
using exam.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models;
using StudentManager.Models.Point;
using StudentManager.Repository;
using StudentManager.Repository.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace exam.Controllers
{
    [Route("api/point-types")]
    public class PointTypeController : Controller
    {
        PointTypeRepository PointTypeRepository;

        public PointTypeController(PointTypeRepository repo)
        {
            this.PointTypeRepository = repo;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var nations = await PointTypeRepository.GetAll();
            if (nations == null || !nations.Any()) return NotFound(new { message = "Không có loại điểm nào" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = nations });
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var nation = await PointTypeRepository.Get(id);
            if (nation == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy loại điểm" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = nation });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] PointType fromBody)
        {
            if (String.IsNullOrEmpty(fromBody.Name))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Tên loại điểm không được để trống"
                });
            if (fromBody.Factor == default(int))
                return BadRequest(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Hệ số điểm không được để trống"
                });
            var exist = await PointTypeRepository.FindByName(fromBody.Name);
            if (exist != null) return BadRequest(new
            {
                status = ResultStatus.STATUS_DUPLICATE,
                message = "Đã có loại điểm này tồn tại trong hệ thống"
            });
            await PointTypeRepository.Create(fromBody);
            return Ok(new
            {
                status = ResultStatus.STATUS_OK,
                data = fromBody
            });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, [FromBody] PointType fromBody)
        {
            var exist = await PointTypeRepository.Get(id);
            if (exist == null) return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy loại điểm" });

            if (String.IsNullOrEmpty(fromBody.Name))
                return BadRequest(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên loại điểm không được để trống" });


            exist.Name = fromBody.Name;
            exist.Factor = fromBody.Factor;

            await PointTypeRepository.Update(id, fromBody);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Sửa thông tin loại điểm thành công", data = exist });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exist = await PointTypeRepository.Get(id);
            if (exist == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy loại điểm" });
            await PointTypeRepository.Delete(id);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Xóa thành công!" });
        }
    }
}

