
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockSchoolManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MockSchoolManagement.Infrastructure
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Student> Students { set; get; }

        public DbSet<Course> Courses { set; get; }

        public DbSet<StudentCourse> StudentCourses { set; get; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();

            var foreignKeys = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            foreach (var key in foreignKeys)
            {
                key.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
