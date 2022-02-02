using MockSchoolManagement.Infrastructure;
using MockSchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.DataRepositories
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly AppDbContext _appDbContext;

        public SqlStudentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Student Add(Student student)
        {
            _appDbContext.Add(student);
            _appDbContext.SaveChanges();

            return student;
        }

        public Student GetStudent(int id)
        {
            return _appDbContext.Students.Find(id);
        }

        public IEnumerable<Student> GetStudents()
        {
            return _appDbContext.Students;
        }
    }
}
