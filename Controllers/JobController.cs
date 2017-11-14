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
    [Route("api/job")]
    public class JobController : Controller
    {
        JobRepository jobRepository;

        public JobController(JobRepository jobRepository)
        {
            this.jobRepository = jobRepository;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var job = await jobRepository.getAll();
            if (job == null || !job.Any()) return NotFound(new { message = "Không có nghề nghiệp nào" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = job });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchoolYear(int id)
        {
            var job = await jobRepository.Get(id);
            if (job == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy nghề nghiệp" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = job });
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] Job job)
        {
            if (String.IsNullOrEmpty(job.Name))
                return Ok(new
                {
                    status = ResultStatus.STATUS_INVALID_INPUT,
                    message = "Tên nghề nghiệp không được để trống"
                });
            await jobRepository.Create(job);
            return Ok(new
            {
                status = ResultStatus.STATUS_OK,
                data = job
            });
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] Job job)
        {
            var user = await jobRepository.Get(job.Id);
            if (user == null) return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy nghề nghiệp" });

            if (String.IsNullOrEmpty(job.Name))
                return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên nghề nghiệp không được để trống" });


            user.Name = job.Name;
            await jobRepository.Update(job.Id, job);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Sửa thông tin nghề nghiệp thành công", data = job });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await jobRepository.Get(id);
            if (user == null)
                return NotFound(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy nghề nghiệp" });
            await jobRepository.Delete(id);
            return Ok(new { status = ResultStatus.STATUS_OK, message = "Xóa thành công!" });
        }

    }
}
