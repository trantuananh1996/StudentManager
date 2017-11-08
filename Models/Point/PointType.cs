using exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Models.Point
{
    public class PointType : BaseModel
    {
        public string Name { get; set; }
        public int Factor { get; set; }
    }
}
