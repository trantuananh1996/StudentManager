using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Models
{
    public class LoginModel : BaseModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
