using cw_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw_3.DAL
{
    public class MockDbService : IDbService
    {
        public static IEnumerable<Student> _students;
        static MockDbService()
        {
            _students = new List<Student>{ 
                new Student{IdStudent = 1, FirstName = "Jan", LastName = "Kowalski"},
                new Student{IdStudent = 2, FirstName = "Robert", LastName = "Lewandowski"},
                new Student{IdStudent = 3, FirstName = "Geralt", LastName = "Riv"},
                new Student{IdStudent = 4, FirstName = "Darth", LastName = "Vader"}
            };
        }
        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}
