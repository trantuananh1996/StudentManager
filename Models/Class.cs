using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Models
{
    public class Class : BaseModel
    {
    
        public string Name { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual SchoolYear SchoolYear { get; set; }
        public int Size { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
