using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models.Reports;
using StudentManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Controllers.Report
{
    [Route("api/semester-result-by-sj")]
    public class SemesterResultBySubjectController : Controller
    {
        SemesterResultBySubjectRepository repo;

        public SemesterResultBySubjectController(SemesterResultBySubjectRepository repo)
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
        [Microsoft.AspNetCore.Authorization.Authorize]

    
    }
}
