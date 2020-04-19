﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw_3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cw_3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        public readonly string conString = "Data Source=db-mssql;Initial Catalog=s19270;Integrated Security=True";
        
        [HttpPost]
        public IActionResult CreateStudent(NewStudent student)
        {
            if (student.Studies == null || student.FirstName == null ||
                student.LastName == null || student.Birthdate == null ||
                student.IndexNumber == null) return NotFound("Brak danych");
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand com = new SqlCommand())
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction("SampleTransaction");
                com.Connection = con;
                com.Transaction = trans;
                try
                {
                    com.CommandText = "select IndexNumber from Student;";
                    SqlDataReader dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        if (student.IndexNumber == dr["IndexNumber"].ToString()) return NotFound("Podany numer indexu juz istnieje");
                    }
                    com.CommandText = "select * from Studies inner join Enrollment on Studies.IdStudy = Enrollment.IdStudy where Name = @studies and Semester = 1;";
                    com.Parameters.AddWithValue("studies", student.Studies);
                    dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        com.CommandText = "insert into Studies(IdStudy, Name) " +
                                            "values((select max(IdStudy) from Studies) + 1, @studies) " +
                                            "insert into Enrollment(IdEnrollment, Semester, IdStudy, StartDate) " +
                                            "values((select max(IdEnrollment) from Enrollment) + 1, 1, (select IdStudy from Studies where Name = @studies), GETDATE())";
                        com.ExecuteNonQuery();
                    }
                    com.CommandText = "insert into Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) " +
                                        "values(@indexnumber, @firstname, @lastname, @birthdate, (select IdEnrollment from Enrollment where IdStudy = (select IdStudy from Studies where Name = @studies and Semester = 1)));";
                    com.Parameters.AddWithValue("indexnumber", student.IndexNumber);
                    com.Parameters.AddWithValue("firstname", student.FirstName);
                    com.Parameters.AddWithValue("lastname", student.LastName);
                    com.Parameters.AddWithValue("birthdate", student.Birthdate);
                    com.ExecuteNonQuery();
                    return Ok("Dodano nowego studenta");
                }
                catch(Exception ex)
                {
                    trans.Rollback("Wystapili bledy");
                }
                return NotFound("Wystapili bledy");
            }
        }
        [Route("api/enrollments/promotions")]
        [HttpGet]
        public IActionResult Promote()
        {
            return Ok("Dziala");
        }
    }
}