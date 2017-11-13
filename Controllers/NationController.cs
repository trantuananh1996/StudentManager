using exam.Repository;
using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Controllers
{
    [Route("api/nation")]
    public class NationController:Controller
    {
        NationRepository nationRepository;

        public NationController(NationRepository nationRepository)
        {
            this.nationRepository = nationRepository;
        }

        
        [HttpGet("nations")]
        public async Task<ActionResult> GetNations(string name)
        {
            var nations = await nationRepository.getAll();
            if (nations==null || !nations.Any())
            {
                return NotFound(new { message="Không có"});
            }

            return Ok(new { status = ResultStatus.STATUS_OK,data=nations});
        }

       
    }
}
