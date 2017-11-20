using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Models
{
    public class LearningCapacities : BaseModel
    {
        public string Name { get; set; }
        public float MinPoint { get; set; }
        public float MaxPoint { get; set; }
        public float ControlPoint { get; set; }
    }
}
