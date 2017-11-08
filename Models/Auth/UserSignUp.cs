using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Models
{
    public class UserSignUp : BaseModel
    {

        [Column(TypeName = "TEXT")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Column(TypeName = "TEXT")]
        [MaxLength(100)]
        public string Email { get; set; }
        [Column(TypeName = "TEXT")]
        [MaxLength(100)]
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; } = 0;
        public string Password_confirm { get; set; }
    }
}
