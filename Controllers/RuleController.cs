using exam.Utils;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models;
using StudentManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Controllers
{
    [Route("api/rule")]
    public class RuleController : Controller
    {
        IRuleRepository ruleRepository;

        public RuleController(IRuleRepository ruleRepository)
        {
            this.ruleRepository = ruleRepository;
        }

        [HttpGet("show")]
        public async Task<ActionResult> GetRule()
        {
            var rule = await ruleRepository.Get(1);
            if (rule == null) return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy quy định" });
            else return Ok(new { status = ResultStatus.STATUS_OK, data = rule });
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdateRule([FromBody] Rule rule)
        {
            if (rule.Id == default(int))
            {
                var existRule = await ruleRepository.Get(1);
                if (existRule == null)
                {
                    await ruleRepository.Create(rule);
                    return Ok(new { status = ResultStatus.STATUS_OK, data = rule });
                }
                else
                {
                    rule.Id = 1;
                    if (rule.MaxAge != default(int)) existRule.MaxAge = rule.MaxAge;
                    if (rule.MinAge != default(int)) existRule.MinAge = rule.MinAge;
                    if (rule.MaxSize != default(int)) existRule.MaxSize = rule.MaxSize;
                    if (rule.MinSize != default(int)) existRule.MinSize = rule.MinSize;
                    if (rule.MaxPoint != default(int)) existRule.MaxPoint = rule.MaxPoint;
                    if (rule.SchoolName != default(string)) existRule.SchoolName = rule.SchoolName;
                    if (rule.SchoolAddress != default(string)) existRule.SchoolAddress = rule.SchoolAddress;
                    await ruleRepository.Update(1, rule);
                    return Ok(new { status = ResultStatus.STATUS_OK, data = rule });
                }
            }
            else
            {
                var existRule = await ruleRepository.Get(rule.Id);
                if (existRule != null)
                {
                    if (rule.MaxAge != default(int)) existRule.MaxAge = rule.MaxAge;
                    if (rule.MinAge != default(int)) existRule.MinAge = rule.MinAge;
                    if (rule.MaxSize != default(int)) existRule.MaxSize = rule.MaxSize;
                    if (rule.MinSize != default(int)) existRule.MinSize = rule.MinSize;
                    if (rule.MaxPoint != default(int)) existRule.MaxPoint = rule.MaxPoint;
                    if (rule.SchoolName != default(string)) existRule.SchoolName = rule.SchoolName;
                    if (rule.SchoolAddress != default(string)) existRule.SchoolAddress = rule.SchoolAddress;
                    await ruleRepository.Update(rule.Id, rule);
                    return Ok(new { status = ResultStatus.STATUS_OK, data = rule });
                }
                else
                {
                    return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy quy định" });
                }
            }
        }
    }
}
