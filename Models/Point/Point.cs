using exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Models.Point
{
    public class Point:BaseModel
    {
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual SchoolYear SchoolYear { get; set; }
        public virtual Class Class { get; set; }
        public virtual PointType PointType { get; set; }
        public float PointNumber { get; set; }
    }
}
