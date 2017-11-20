using exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Models.Post
{
    public class PostCreateClass:BaseModel
    {
        public string Name { get; set; }
        public int GradeId { get; set; }
        public int SchoolYearId { get; set; }
        public int Size { get; set; }
        public int TeacherId { get; set; }
    }
}
