using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace School.Controllers    
{
    [Route("api/Course")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        // Dependency injection of the database context
        public CourseAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all courses in the database
        /// </summary>
        /// <example>
        /// GET: api/Course/ListCourses -> [{"CourseId":1,"CourseCode":"http5101",...},...]
        /// </example>
        /// <returns>
        /// A list of Course objects
        /// </returns>
        [HttpGet]
        [Route("ListCourses")]
        public List<Course> ListCourses()
        {
            // create an empty list to hold Course objects
            List<Course> Courses = new List<Course>();

            // using statement ensures the connection is closed after execution
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // open the connection
                Connection.Open();

                // create a new SQL command
                MySqlCommand Command = Connection.CreateCommand();

                // define the SQL query to retrieve all courses
                Command.CommandText = "SELECT * FROM courses";

                // execute the query and store the result set
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // loop through each row in the result set
                    while (ResultSet.Read())
                    {
                        // access column information by the course table property/column name as an index
                        int CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        int TeacherId =  Convert.ToInt32(ResultSet["teacherid"]);
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string CourseName = ResultSet["coursename"].ToString();

                        // create a new Course object and populate its properties
                        Course CurrentCourse = new Course()
                        {
                            CourseId = CourseId,
                            CourseCode = CourseCode,
                            TeacherId = TeacherId,
                            StartDate = StartDate,
                            FinishDate = FinishDate,
                            CourseName = CourseName
                        };

                        // add the Course object to the list
                        Courses.Add(CurrentCourse);
                    }
                }
            }

            // return the list of courses
            return Courses;
        }

        /// <summary>
        /// Returns a course from the database based on its ID
        /// </summary>
        /// <example>
        /// GET: api/Course/FindCourse/3 -> {"CourseId":3,"CourseCode":"http5103",...}
        /// </example>
        /// <param name="id">The ID of the course to find</param>
        /// <returns>
        /// a Course object matching the provided id, will return a course object with null property values if course id is not in the database
        /// </returns>
        [HttpGet]
        [Route("FindCourse/{id}")]
        public Course FindCourse(int id)
        {
            // create an empty Course object
            Course SelectedCourse = new Course();

            // using statement ensures the connection is closed after execution
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // open the connection
                Connection.Open();

                // create a new SQL command
                MySqlCommand Command = Connection.CreateCommand();

                // define the SQL query with a parameter for the course ID
                Command.CommandText = "SELECT * FROM courses WHERE courseid = @id";
                Command.Parameters.AddWithValue("@id", id);

                // execute the query and store the result set
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // loop through each row of the result set
                    while (ResultSet.Read())
                    {
                        // access column information by the course table property/column name as an index
                        SelectedCourse.CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        SelectedCourse.CourseCode = ResultSet["coursecode"].ToString();
                        SelectedCourse.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        SelectedCourse.StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        SelectedCourse.FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        SelectedCourse.CourseName = ResultSet["coursename"].ToString();
                    }
                }
            }

            // return the Course object
            return SelectedCourse;
          

        }

		/// <summary>
		/// Adds a new course to the database.
		/// </summary>
		/// <param name="NewCourse">A Course object containing details of the new course.</param>
		/// <example>
		/// POST: api/Course/AddCourse
		/// Body:
		/// {
		///     "CourseCode": "HTTP5888",
		///     "TeacherId": 1,
		///     "StartDate": "2025-01-08",
		///     "FinishDate": "2024-05-14",
		///     "CourseName": "Web Entrepreneurship"
		/// }
		/// </example>
		/// <returns>
		/// The ID of the newly added course if successful.
		/// </returns>
		[HttpPost]
		[Route("AddCourse")]
		public int AddCourse([FromBody] Course NewCourse)
		{
            // 'using' will close the connection after the code executes 
            using (MySqlConnection Connection = _context.AccessDatabase())
			{
				// open connection
                Connection.Open();

                // create a new sql command
				MySqlCommand Command = Connection.CreateCommand();


                // sql query to insert new course properties into the courses table in the database
                Command.CommandText = "INSERT INTO courses (coursecode, teacherid, startdate, finishdate, coursename) " +
									  "VALUES (@CourseCode, @TeacherId, @StartDate, @FinishDate, @CourseName)";
				Command.Parameters.AddWithValue("@CourseCode", NewCourse.CourseCode);
				Command.Parameters.AddWithValue("@TeacherId", NewCourse.TeacherId);
				Command.Parameters.AddWithValue("@StartDate", NewCourse.StartDate);
				Command.Parameters.AddWithValue("@FinishDate", NewCourse.FinishDate);
				Command.Parameters.AddWithValue("@CourseName", NewCourse.CourseName);

                // command that returns the number of rows affected by query
                Command.ExecuteNonQuery();

                // return the last inserted teacher id converted to int
                return Convert.ToInt32(Command.LastInsertedId);
			}

            //if failure return 0

            return 0;
		}

		/// <summary>
		/// Deletes a course from the database by its ID.
		/// </summary>
		/// <param name="id">The ID of the course to delete.</param>
		/// <example>
		/// DELETE: api/Course/DeleteCourse/1
		/// </example>
		/// <returns>
		/// The number of rows affected by the delete operation.
		/// </returns>
		[HttpDelete]
		[Route("DeleteCourse/{id}")]
		public int DeleteCourse(int id)
		{
            // 'using' will close the connection after the code executes 
            using (MySqlConnection Connection = _context.AccessDatabase())
			{
				Connection.Open();
				MySqlCommand Command = Connection.CreateCommand();


                // sql query to delete from the courses table, the course with the matching course id 
                Command.CommandText = "DELETE FROM courses WHERE courseid = @CourseId";
				Command.Parameters.AddWithValue("@CourseId", id);


                // command that returns the number of rows affected by query
                return Command.ExecuteNonQuery();
			}

			//if failure return 0

			return 0;
		}

		/// <summary>
		/// Updates a course's information in the database.
		/// </summary>
		/// <param name="id">The ID of the course to update.</param>
		/// <param name="CourseData">A Course object with updated information.</param>
		/// <example>
		/// PUT: api/Course/UpdateCourse/1
		/// Body:
		/// {
		///     "CourseCode": "HTTP5888",
		///     "TeacherId": 1,
		///     "StartDate": "2025-01-08",
		///     "FinishDate": "2024-05-14",
		///     "CourseName": "Web Entrepreneurship"
		/// }
		/// </example>
		/// <returns>
		/// The updated Course object or an appropriate error message if the update fails.
		/// </returns>
		[HttpPut]
		[Route("UpdateCourse/{id}")]
		public IActionResult UpdateCourse(int id, [FromBody] Course CourseData)
		{
			using (MySqlConnection Connection = _context.AccessDatabase())
			{
				Connection.Open();
				MySqlCommand Command = Connection.CreateCommand();


				// Parameterized SQL query to update the course
				Command.CommandText = "UPDATE courses SET coursecode = @CourseCode, teacherid = @TeacherId, " +
									  "startdate = @StartDate, finishdate = @FinishDate, coursename = @CourseName " +
									  "WHERE courseid = @CourseId";
				Command.Parameters.AddWithValue("@CourseCode", CourseData.CourseCode);
				Command.Parameters.AddWithValue("@TeacherId", CourseData.TeacherId);
				Command.Parameters.AddWithValue("@StartDate", CourseData.StartDate);
				Command.Parameters.AddWithValue("@FinishDate", CourseData.FinishDate);
				Command.Parameters.AddWithValue("@CourseName", CourseData.CourseName);
				Command.Parameters.AddWithValue("@CourseId", id);

				int RowsAffected = Command.ExecuteNonQuery();

				if (RowsAffected == 0)
				{
					return NotFound("Course not found.");
				}
			}

			return Ok(FindCourse(id)); // Return the updated course
		}

	}
}