using System;
using System.Threading.Tasks;
using exam.Models;
using exam.Repository;
using Microsoft.AspNetCore.Mvc;

namespace exam.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly UserRepository _user;
        public UserController(UserRepository user)
        {
            _user = user;
        }
        /// <summary>
        /// Get list users
        /// </summary>
        /// <returns>The list.</returns>
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        [HttpGet("list")]
        public async Task<IActionResult> List(int perpage,int page = 1)
        {
            if(perpage == 0)
            {
                var users = await _user.getAll();
                return Ok(users);
            } else {
                var users = await _user.Paginate(perpage, page);
                return Ok(users);
            }


        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _user.Get(id);
            return Ok(user);
        }

        /// <summary>
        /// Create New user
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="u">U.</param>
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] User u)
        {
             await _user.Create(u);
            return Ok(new
            {
                msg = "Create Success!",
                user = u
            });
        }

        /// <summary>
        /// Edit User
        /// </summary>
        /// <returns>The edit.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="u">U.</param>
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id,String password)
        {
            var user = await _user.Get(id);
            if (user == null) return NotFound();
            user.password = password;
            await _user.Update(id, user);
            return Ok(new { msg = "Updated!",user = user });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _user.Get(id);
            if (user == null) return NotFound();
            await _user.Delete(id);
            return Ok(new { msg = "Deleted!" });
        }

    }
}
