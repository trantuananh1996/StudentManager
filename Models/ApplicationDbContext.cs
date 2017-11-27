using System;
using Microsoft.EntityFrameworkCore;
using StudentManager.Models.Point;
using StudentManager.Models;
using StudentManager.Models.Reports;

namespace exam.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt)
            : base(opt)
        {
        }
   
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<PointType> PointTypes { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<SchoolYear> SchoolYears { get; set; }
        public DbSet<Nation> Nations { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Religion> Religions { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Conduct> Conducts { get; set; }
        public DbSet<LearningCapacities> LearningCapacities { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<SemesterResult> SemesterResults { get; set; }
        public DbSet<SemesterResultBySubject> SemesterResultBySubjects { get; set; }
        public DbSet<YearResult> YearResults { get; set; }
        public DbSet<YearResultBySubject> YearResultSubjects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define composite key.
            modelBuilder.Entity<SemesterResult>()
                .HasKey(lc => new { lc.StudentId, lc.ClassId ,lc.SchoolYearId,lc.SemesterId});
            modelBuilder.Entity<SemesterResultBySubject>()
            .HasKey(lc => new { lc.StudentId, lc.ClassId, lc.SchoolYearId, lc.SemesterId });
            modelBuilder.Entity<YearResult>()
            .HasKey(lc => new { lc.StudentId, lc.ClassId, lc.SchoolYearId });
            modelBuilder.Entity<YearResultBySubject>()
            .HasKey(lc => new { lc.StudentId, lc.ClassId, lc.SchoolYearId });
        }
    }
}
