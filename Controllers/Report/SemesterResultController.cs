﻿using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Controllers.Report
{

    [Route("api/semester-result")]
    public class SemesterResultController : Controller
    {
        SemesterResultRepository repo;

        public SemesterResultController(SemesterResultRepository repo)
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
