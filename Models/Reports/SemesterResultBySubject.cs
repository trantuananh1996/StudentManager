using exam.Models;
using StudentManager.Models.Point;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.Models.Reports
{
    public class SemesterResultBySubject:BaseModel

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

        [ForeignKey("Semester")]
        [Key]
        public int SemesterId { get; set; }
        public Semester Semester { get; set; }

        [ForeignKey("SchoolYear")]
        [Key]
        public int SchoolYearId { get; set; }
        public SchoolYear SchoolYear { get; set; }

        public float AverageExamScore { get; set; }
        public float AverageSubjectScore { get; set; }
    }
}
