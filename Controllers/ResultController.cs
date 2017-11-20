using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Repository;
using System.Linq;
using System.Threading.Tasks;

using exam.Models;
using System;
using System.Collections.Generic;


namespace StudentManager.Controllers
{
    [Route("api/result")]
    public class ResultController : Controller
    {

        ResultRepository resultRepository;

        public ResultController(ResultRepository resultRepository)
        {
            this.resultRepository = resultRepository;
        }

        [HttpGet("results")]
        public async Task<ActionResult> GetResults(string name)
        {
            var results = await resultRepository.getAll();
            if (results == null || !results.Any())
            {
                return NotFound(new { message = "Không tìm thấy dữ liệu" });
            }

            return Ok(new { status = ResultStatus.STATUS_OK, data = results });
        }
    }
}
