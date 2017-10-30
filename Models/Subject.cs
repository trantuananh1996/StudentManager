using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Models
{
    public class Subject : BaseModel
    {
        public string Name { get; set; }
        public int LessionSize { get; set; }
        public int Factor { get; set; }
    }
}
