using exam.Repository;
using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Controllers
{
    [Route("api/conduct")]
    public class ConductController : Controller
    {
        ConductRepository conductRepository;

        public ConductController(ConductRepository conductRepository)
        {
            this.conductRepository = conductRepository;
        }


        [HttpGet("conducts")]
        public async Task<ActionResult> GetConducts(string name)
        {
            var conducts = await conductRepository.getAll();
            if (conducts == null || !conducts.Any())
            {
                return NotFound(new { message = "Không có dữ liệu" });
            }

            return Ok(new { status = ResultStatus.STATUS_OK, data = conducts });
        }


    }
}
