using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Models
{
    public class BaseModel
    {
        [Column(Order = 0)]
        public int Id { get; set; }
    }
}
