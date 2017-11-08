using exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Models
{
    public class Rule:BaseModel
    {
        public int MaxAge { get; set; }
        public int MinAge { get; set; }
        public int MaxSize { get; set; }
        public int MinSize { get; set; }
        public int MaxPoint { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
    }
}
