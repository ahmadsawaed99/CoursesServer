using System;
using CourseManagement.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.Entity<Student_Course>().HasOne(b => b.Course).WithMany(c => c.Students).HasForeignKey(ci => ci.CourseId);

            builder.Entity<Student_Course>().HasOne(b => b.User).WithMany(c => c.Students_Course).HasForeignKey(ci => ci.UserId);

        }

        public DbSet<Courses> Courses { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<Student_Course> Students_Course { get; set; }

        public DbSet<Student_Class> Students_Class { get; set; }

        public DbSet<CourseTiming> CoursesTiming { get; set; }
    }
}
