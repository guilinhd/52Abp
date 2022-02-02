using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockSchoolManagement.Models;
using MockSchoolManagement.Models.EnumTypes;

namespace MockSchoolManagement.Infrastructure
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = 1,
                    Name = "张三",
                    Major = MajorEnum.ComputerScience,
                    Email = "zhangsan@hotmail.com"
                }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = 2,
                    Name = "李四",
                    Major = MajorEnum.ElectronicCommerce,
                    Email = "lisi@hotmail.com"
                }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = 3,
                    Name = "王五",
                    Major = MajorEnum.Mathematics,
                    Email = "wangwu@hotmail.com"
                }
            );
        }
    }
}
