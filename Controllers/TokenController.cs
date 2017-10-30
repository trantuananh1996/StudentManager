using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using exam.Models;
using exam.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace exam.Controllers
{
    [Route("api/v1/token")]
    public class HomeController : Controller
    {
        private IUserRepository _iUserRepository;
        private string secrectKey = "needtogetthisfromenvironment";
        public HomeController(IUserRepository iUserRepository)
        {
            _iUserRepository = iUserRepository;
        }

        [HttpPost]
        public IActionResult Token([FromBody] User model)
        {
            var user = _iUserRepository.FindByEmail(model.email);
            if(user == null || user.password != model.password){
                return BadRequest();
            } else {
                
            }
            var token = genToken(user);
            return Ok(new { token = genToken(user)});

        }

        private string genToken(User u){
            
            var key = new SymmetricSecurityKey(Encoding.Default.GetBytes(this.secrectKey));
            var claims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.Sub, u.email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")        
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires:DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key,SecurityAlgorithms.HmacSha256)
            );
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
           
            return tokenStr;
        }
    }
}
