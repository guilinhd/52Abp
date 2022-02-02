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

        public Student Insert(Student student)
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

        public Student Update(Student student)
        {
            var updateStudent = _appDbContext.Attach<Student>(student);
            updateStudent.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _appDbContext.SaveChanges();

            return student;
        }

        public Student Delete(int id)
        {
            Student student = _appDbContext.Students.Find(id);
            if (student != null)
            {
                _appDbContext.Students.Remove(student);
                _appDbContext.SaveChanges();
            }

            return student;
        }
    }
}
