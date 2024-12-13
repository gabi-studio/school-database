using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;
using System.Diagnostics;

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

        /// <summary>
        /// Adds a new student to the database.
        /// </summary>
        /// <param name="NewStudent">A Student object containing details of the new student.</param>
        /// <example>
        /// POST: api/Student/AddStudent
        /// Body:
        /// {
        ///     "StudentFName": "Harry",
        ///     "StudentLName": "Potter",
        ///     "StudentNumber": "S1234",
        ///     "EnrollDate": "2025-01-01"
        /// }
        /// </example>
        /// <returns>
        /// The ID of the newly added student if successful.
        /// </returns>
        [HttpPost]
        [Route("AddStudent")]
        public int AddStudent([FromBody] Student NewStudent)
        {
            // 'using' will close the connection after the code executes 
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // open connection
                Connection.Open();

                // Create a new sql command
                MySqlCommand Command = Connection.CreateCommand();


                // sql command to insert new student properties into the students table in the database
                Command.CommandText = "INSERT INTO students (studentfname, studentlname, studentnumber, enroldate) " +
                                      "VALUES (@FirstName, @LastName, @StudentNumber, @EnrollDate)";
                Command.Parameters.AddWithValue("@FirstName", NewStudent.StudentFName);
                Command.Parameters.AddWithValue("@LastName", NewStudent.StudentLName);
                Command.Parameters.AddWithValue("@StudentNumber", NewStudent.StudentNumber);
                Command.Parameters.AddWithValue("@EnrollDate", NewStudent.EnrollDate);

                // command that returns the number of rows affected by query
                Command.ExecuteNonQuery();

                // return the last inserted teacher id converted to int
                return Convert.ToInt32(Command.LastInsertedId);
            }

            // if fail return 0

            return 0;
        }



        /// <summary>
        /// Deletes a student from the database by their ID.
        /// </summary>
        /// <param name="id">The ID of the student to delete.</param>
        /// <example>
        /// DELETE: api/Student/DeleteStudent/1
        /// </example>
        /// <returns>
        /// The number of rows affected by the delete operation.
        /// </returns>
        [HttpDelete]
        [Route("DeleteStudent/{id}")]
        public int DeleteStudent(int id)

        {
            // 'using' will close the connection after the code executes 
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                //open connection
                Connection.Open();

                // create new sql command
                MySqlCommand Command = Connection.CreateCommand();

                // sql query to delete from the students table, the student with the matching student id 
                Command.CommandText = "DELETE FROM students WHERE studentid = @StudentId";
                Command.Parameters.AddWithValue("@StudentId", id);

                // command that returns the number of rows affected by query
                return Command.ExecuteNonQuery();
            }

            // if fail return 0

            return 0;
        }

		/// <summary>
		/// Updates a student's information in the database.
		/// </summary>
		/// <param name="id">The ID of the student to update.</param>
		/// <param name="StudentData">A Student object with updated information.</param>
		/// <example>
		/// PUT: api/Student/UpdateStudent/1
		/// Body:
		/// {
		///     "StudentFName": "Harry",
		///     "StudentLName": "Potter",
		///     "StudentNumber": "S1234",
		///     "EnrollDate": "2025-01-01"
		/// }
		/// </example>
		/// <returns>
		/// The updated Student object or an appropriate error message if the update fails.
		/// </returns>
		[HttpPut]
		[Route("UpdateStudent/{id}")]
		public IActionResult UpdateStudent(int id, [FromBody] Student StudentData)
		{
			using (MySqlConnection Connection = _context.AccessDatabase())
			{
				Connection.Open();
				MySqlCommand Command = Connection.CreateCommand();


				// Parameterized SQL query to update the student
				Command.CommandText = "UPDATE students SET studentfname = @StudentFName, studentlname = @StudentLName, " +
									  "studentnumber = @StudentNumber, enroldate = @EnrollDate WHERE studentid = @StudentId";
				Command.Parameters.AddWithValue("@StudentFName", StudentData.StudentFName);
				Command.Parameters.AddWithValue("@StudentLName", StudentData.StudentLName);
				Command.Parameters.AddWithValue("@StudentNumber", StudentData.StudentNumber);
				Command.Parameters.AddWithValue("@EnrollDate", StudentData.EnrollDate);
				Command.Parameters.AddWithValue("@StudentId", id);

				int RowsAffected = Command.ExecuteNonQuery();

				if (RowsAffected == 0)
				{
					return NotFound("Student not found.");
				}
			}

			return Ok(FindStudent(id)); // Return the updated student
		}


	}
}