using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentManager.Repository;
using exam.Utils;

namespace StudentManager.Controllers.Report
{
    [Route("api/year-result-by-subject")]
    public class YearResultBySubjectController : Controller
    {
        YearResultBySubjectRepository repo;

        public YearResultBySubjectController(YearResultBySubjectRepository repo)
        {
            this.repo = repo;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var result = await repo.GetAll();
            if (result == null || !result.Any()) return NotFound(new { message = "Không có kết quả nào" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = result });
        }

    }
}
