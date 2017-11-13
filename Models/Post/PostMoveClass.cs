using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Models.Post
{
    public class PostMoveClass
    {
        public int SourceClass { get; set; }
        public int TargetClass { get; set; }
        public List<int> StudentIds { get; set; }
    }
}
