using exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Models.Point
{
    public class ListPoint:BaseModel
    {
        public int PointTypeId { get; set; }
        public float Point { get; set; }
    }
}
