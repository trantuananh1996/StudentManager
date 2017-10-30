using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Models.Post
{
    public class PostAddStudentClass
    {
        public int ClassId { get; set; }
        public List<int> StudentIds { get; set; }
    }
}
