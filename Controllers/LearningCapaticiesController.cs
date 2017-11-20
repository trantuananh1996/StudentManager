using exam.Repository;
using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Controllers
{
    [Route("api/learningcapaticie")]
    public class LearningCapaticiesController : Controller
    {
        LearningCapacitiesRepository learningCapacitiesRepository;

        public LearningCapaticiesController(LearningCapacitiesRepository learningCapacitiesRepository)
        {
            this.learningCapacitiesRepository = learningCapacitiesRepository;
        }


        [HttpGet("learningcapaticies")]
        public async Task<ActionResult> GetLearningcapacities(string name)
        {
            var learning = await learningCapacitiesRepository.getAll();
            if (learning == null || !learning.Any())
            {
                return NotFound(new { message = "Không có dữ liệu" });
            }

            return Ok(new { status = ResultStatus.STATUS_OK, data = learning });
        }


    }
}
