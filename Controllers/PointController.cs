using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models.Point;
using StudentManager.Repository.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Controllers
{
    [Route("api/point")]
    public class PointController:Controller
    {
        IPointTypeRepository pointTypeRepository;
        IPointRepository pointRepository;

        public PointController(IPointTypeRepository pointTypeRepository, IPointRepository pointRepository)
        {
            this.pointTypeRepository = pointTypeRepository;
            this.pointRepository = pointRepository;
        }

        [HttpGet("point-types")]
        public async Task<ActionResult> GetPointTypes()
        {
            List<PointType> pointtypes = await pointTypeRepository.getAll();
            if (pointtypes == null || pointtypes.Count() == 0)
                return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không có loại điểm nào" });
            else
                return Ok(new {status = ResultStatus.STATUS_OK,data= pointtypes });
        }
    }
}
