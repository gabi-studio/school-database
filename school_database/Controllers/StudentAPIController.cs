using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace School.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        // Dependency injection of the database context
        public StudentAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all students in the database
        /// </summary>
        /// <example>
        /// GET: api/Student/ListStudents -> [{"StudentId":1,"StudentFName":"Sarah", "StudentLName":"Valdez",...},...]
        /// </example>
        /// <returns>
        /// A list of Student objects (later linked to the /StudentPage/Show/{id} view)
        /// </returns>
        [HttpGet]
        [Route("ListStudents")]
        public List<Student> ListStudents()
        {
            // create an empty list to hold Student objects
            List<Student> Students = new List<Student>();

            // using statement ensures the connection is closed after execution
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // open the connection
                Connection.Open();

                // create a new SQL command
                MySqlCommand Command = Connection.CreateCommand();

                // define the SQL query to retrieve all students
                Command.CommandText = "SELECT * FROM students";

                // execute the query and store the result set
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // loop through each row in the result set
                    while (ResultSet.Read())
                    {
                        // Map each column to the properties of the Student object
                        int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrollDate = Convert.ToDateTime(ResultSet["enroldate"]);

                        // access column information by the student table property/column name as an index
                        Student CurrentStudent = new Student()
                        {
                            StudentId = StudentId,
                            StudentFName = FirstName,
                            StudentLName = LastName,
                            StudentNumber = StudentNumber,
                            EnrollDate = EnrollDate
                        };

                        // add the Student object to the list
                        Students.Add(CurrentStudent);
                    }
                }
            }

            // return the list of students
            return Students;
        }

        /// <summary>
        /// Returns a student from the database based on their ID
        /// </summary>
        /// <example>
        /// GET: api/Student/FindStudent/3 -> {"StudentId":3,"StudentFName":"Austin","StudentLName":"Simon",...}
        /// </example>
        /// <param name="id">The ID of the student to find</param>
        /// <returns>
        /// A Student object matching the provided id, will return a student object with null property values if studen id is not in the database
        /// </returns>
        [HttpGet]
        [Route("FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            // create an empty Student object
            Student SelectedStudent = new Student();

            // using statement ensures the connection is closed after execution
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // open the connection
                Connection.Open();

                // create a new SQL command
                MySqlCommand Command = Connection.CreateCommand();

                // define the SQL query with a parameter for the student ID
                Command.CommandText = "SELECT * FROM students WHERE studentid = @id";
                Command.Parameters.AddWithValue("@id", id);

                // execute the query and store the result set
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // loop through each row in the result set
                    while (ResultSet.Read())
                    {
                        // access column information by the student table property/column name as an index
                        SelectedStudent.StudentId = Convert.ToInt32(ResultSet["studentid"]);
                        SelectedStudent.StudentFName = ResultSet["studentfname"].ToString();
                        SelectedStudent.StudentLName = ResultSet["studentlname"].ToString();
                        SelectedStudent.StudentNumber = ResultSet["studentnumber"].ToString();
                        SelectedStudent.EnrollDate = Convert.ToDateTime(ResultSet["enroldate"]);
                    }
                }
            }

            // Return the Student object (it will be empty if no student was found)
            return SelectedStudent;
        }
    }
}