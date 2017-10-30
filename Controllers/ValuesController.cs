using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using exam.Models;
using exam.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace exam.Controllers
{   
 
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public IUserRepository _users { get; set; }
        public ValuesController(IUserRepository users)
        {
            _users = users;
        }


        // GET api/values
        [HttpGet]
        public List<User> Get()
        {
            return _users.getAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            User u = new User
            {
                name = "Minh Phu Nguyen",
                email = "phuapple.uet@gmail.com",
                username = "phunm"
            };
            if(_users.Create(u) == null){
                return "bbb";
            }

            return "aaa";
        }

        // POST api/values
        [HttpPost]
        [Route("sign-up")]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
