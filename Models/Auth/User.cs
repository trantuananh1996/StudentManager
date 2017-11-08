using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exam.Models
{
    public class User : BaseModel
    {
        [Column(TypeName = "TEXT")]
        [MaxLength(100)]
        public string name { get; set; }
        [Column(TypeName = "TEXT")]
        [MaxLength(100)]
        public string email { get; set; }
        [Column(TypeName = "TEXT")]
        [MaxLength(100)]
        public string username { get; set; }
        [JsonIgnore]
        public string password { get; set; }
        public virtual Role Role { get; set; } 
        public int IsLocked { get; set; }
    }
}
