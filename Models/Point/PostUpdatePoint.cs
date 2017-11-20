using exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Models.Point
{
    public class PostUpdatePoint:BaseModel
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int SemesterId { get; set; }
        public int SchoolYearId { get; set; }
        public int ClassId { get; set; }
        public int PointTypeId { get; set; }
        public float PointNumber { get; set; }
    }
}
