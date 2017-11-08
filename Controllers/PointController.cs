using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models;
using StudentManager.Models.Point;
using StudentManager.Repository;
using StudentManager.Repository.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Controllers
{
    [Route("api/point")]
    public class PointController : Controller
    {
        IPointTypeRepository pointTypeRepository;
        IPointRepository pointRepository;
        IRuleRepository ruleRepository;
        public PointController(IPointTypeRepository pointTypeRepository, IPointRepository pointRepository, IRuleRepository ruleRepository)
        {
            this.pointTypeRepository = pointTypeRepository;
            this.pointRepository = pointRepository;
            this.ruleRepository = ruleRepository;
        }
        [HttpGet("point-types")]
        public async Task<ActionResult> GetPointTypes()
        {
            List<PointType> pointtypes = await pointTypeRepository.getAll();
            if (pointtypes == null || pointtypes.Count() == 0)
                return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không có loại điểm nào" });
            else
                return Ok(new { status = ResultStatus.STATUS_OK, data = pointtypes });
        }

        private async Task<bool> IsPointInvalid(float point)
        {
            Rule rule = await ruleRepository.GetDefaultRule();
            int maxPoint = rule == null ? 10 : rule.MaxPoint;
            if (point < 0 || point > maxPoint) return true;
            else return false;
        }

        [HttpPost("update-point")]
        public async Task<ActionResult> CreateOrUpdate([FromBody] Point point)
        {
            if (await IsPointInvalid(point.PointNumber))
            {
                return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Số điểm không hợp lệ" });
            }
            else
            if (point.Id == default(int))
            {
                await pointRepository.Create(point);
                return Ok(new { status = ResultStatus.STATUS_OK, data = point });
            }
            else
            {
                var existPoint = await pointRepository.Get(point.Id);
                if (existPoint == null)
                {
                    await pointRepository.Create(point);
                    return Ok(new { status = ResultStatus.STATUS_OK, data = point });
                }
                else
                {
                    if (point.Subject != null) existPoint.Subject = point.Subject;
                    existPoint.Student = point.Student;
                    existPoint.Semester = point.Semester;
                    existPoint.SchoolYear = point.SchoolYear;
                    existPoint.PointType = point.PointType;
                    existPoint.PointNumber = point.PointNumber;
                    return Ok(new { status = ResultStatus.STATUS_OK, data = point });
                }
            }
        }
        [HttpPost("update-point")]
        public async Task<ActionResult> CreateOrUpdate([FromBody] List<Point> points)
        {
            foreach (Point p in points)
            {
                await CreateOrUpdate(p);
            }
            return Ok(new { status = ResultStatus.STATUS_OK, data = points });
        }
    }
}
