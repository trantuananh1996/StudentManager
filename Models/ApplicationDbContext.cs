using System;
using Microsoft.EntityFrameworkCore;
using StudentManager.Models.Point;
using StudentManager.Models;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
