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
   
        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Student> students { get; set; }
        public DbSet<StudentClass> studentClasses { get; set; }
        public DbSet<Class> classes { get; set; }
        public DbSet<Grade> grades { get; set; }
        public DbSet<Rule> rules { get; set; }
        public DbSet<Point> points { get; set; }
        public DbSet<PointType> pointTypes { get; set; }
        public DbSet<Semester> semesters { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
