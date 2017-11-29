using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using exam.Models;
using exam.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using exam.Utils;

namespace exam.Controllers
{

    [Route("api/auth")]
    public class AuthController : Controller
    {
        private UserRepository _iUserRepository;
        private RoleRepository roleRepository;
        private string secrectKey = "needtogetthisfromenvironment";
        private int expiredMinute = 20;
        public AuthController(UserRepository iUserRepository, RoleRepository role)
        {
            _iUserRepository = iUserRepository;
            roleRepository = role;
        }

        [HttpGet]
        [Route("roles")]
        public async Task<IActionResult> Roles()
        {
            var roles = await roleRepository.GetAll();
            return Ok(new { status = ResultStatus.STATUS_OK, data = roles });
        }

        [HttpPost]
        [Route("sign-up")]
        public async Task<IActionResult> SignUp([FromBody]UserSignUp newU)
        {
            if (String.IsNullOrEmpty(newU.Name)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Thiếu họ tên người dùng" });
            if (String.IsNullOrEmpty(newU.Username) || String.IsNullOrEmpty(newU.Password)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Thiếu tên đăng nhập hoặc mật khẩu" });
            if (String.IsNullOrEmpty(newU.Password) || String.IsNullOrEmpty(newU.Password)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Mật khẩu không được để trống" });
            if (newU.Password.Length < 6) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Mật khẩu không được ngắn hơn 6 kí tự" });

            if (!newU.Password.Equals(newU.Password_confirm))
            {
                return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Xác nhận mật khẩu không khớp" });
            }
            var tmp1 = await _iUserRepository.FindByEmail(newU.Email);
            if (tmp1 != null)
            {
                return Ok(new { status = ResultStatus.STATUS_DUPLICATE, message = "Email đã có người đăng kí" });

            }
            var tmp2 = await _iUserRepository.FindByUsername(newU.Username);
            if (tmp2 != null)
            {
                return Ok(new { status = ResultStatus.STATUS_DUPLICATE, message = "Username đã có người đăng kí" });
            }
            if (newU.Role.Equals(RoleCode.ROLE_ADMIN))
            {
                return Ok(new { status = ResultStatus.STATUS_FOBIDDEN, message = "Không thể tạo tài khoản mang quyền Admin" });
            }
            var role = await roleRepository.FindRoleById(newU.Role);
            if (role == null) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Không tìm thấy quyền" });
            var user = new User
            {
                name = newU.Name,
                username = newU.Username,
                email = newU.Email,
                password = newU.Password,
                IsLocked = 0,
                Role = role
            };

            await _iUserRepository.Create(user);
            return Ok(new { status = 1, data = user });
        }

        [HttpPost]
        [Route("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] LoginModel loginModel)
        {
            var user = await _iUserRepository.FindByUsername(loginModel.Username);
            if (user == null || !user.password.Equals(loginModel.Password))
            {
                return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Sai tên đăng nhập hoặc mật khẩu" });
            }
            if (user.IsLocked.Equals(1))
            {
                return Ok(new { status = ResultStatus.STATUS_FOBIDDEN, message = "Tài khoản bị khóa" });
            }
            var token = GenToken(user);
            return Ok(new { status = ResultStatus.STATUS_OK, data = new { token = GenToken(user), user = user } });
        }

        private string GenToken(User u)
        {

            var key = new SymmetricSecurityKey(Encoding.Default.GetBytes(this.secrectKey));
            var claims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.NameId, u.email),
                new Claim(JwtRegisteredClaimNames.Jti,u.email),

                new Claim(ClaimTypes.Role,u.Role.CodeName),
                new Claim("userid",u.Id + ""),
                new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenStr;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> UserInfo()
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == "userid").Value;
            if (userid == null) return BadRequest(new { message = "Không tìm thấy người dùng" });
            User u = await _iUserRepository.Get(Int16.Parse(userid));
            if (u == null) return BadRequest(new { message = "Không tìm thấy người dùng" });
            return Ok(new { status = ResultStatus.STATUS_OK, data = u });
        }
    }
}