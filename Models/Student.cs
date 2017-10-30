using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Models
{
    public class Student : BaseModel
    {
        public string FullName { get; set; }
        public int Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string PlaceOfBirth { get; set; }
        public virtual Nation Nation { get; set; }
        public virtual Religion Religion { get; set; }
        public string FatherName { get; set; }
      //  public int FatherJobId { get; set; }
        //public virtual Job FatherJob { get; set; }
        public string MotherName { get; set; }
       // public virtual Job MotherJob { get; set; }
    }
}
