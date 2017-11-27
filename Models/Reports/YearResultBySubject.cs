using exam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Models.Reports
{
    public class YearResultBySubject
    {
        [ForeignKey("Student")]
        [Key]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Class")]
        [Key]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [ForeignKey("Subject")]
        [Key]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        [ForeignKey("SchoolYear")]
        [Key]
        public int SchoolYearId { get; set; }
        public SchoolYear SchoolYear { get; set; }

        public float ReExamScore { get; set; }
        public float AverageScore { get; set; }
    }
}
