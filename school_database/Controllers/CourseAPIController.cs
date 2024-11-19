using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

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
                        int CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string CourseName = ResultSet["coursename"].ToString();
                    }
                }
            }

            // return the Course object
            return SelectedCourse;
        }
    }
}