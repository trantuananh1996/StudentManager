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
    [Route("api/semester")]
    public class SemesterController : Controller
    {

        SemesterRepository semesterRepository;

        public SemesterController(SemesterRepository semesterRepository)
        {
            this.semesterRepository = semesterRepository;
        }

        [HttpGet("semesters")]
        public async Task<ActionResult> GetSemesters(string name)
        {
            var semesters = await semesterRepository.getAll();
            if (semesters == null || !semesters.Any())
            {
                return NotFound(new { message = "Không tìm thấy học kỳ" });
            }

            return Ok(new { status = ResultStatus.STATUS_OK, data = semesters });
        }
    }
}
