using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Models;
using MySql.Data.MySqlClient;
using System;


namespace School.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        // Dependency injection of database context
        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all the teachers in the database
        /// </summary>
        /// <example>
        /// GET: api/TeacherPage/List -> [{"TeacherId":1,"TeacherFName":"Alexander", "TeacherLName":"Bennett",...},...]
        /// </example>
        /// <returns>
        /// A list of teacher objects (which will each then be later linked to the /TeacherPage/Show/{id})
        /// </returns>
        [HttpGet]
        [Route("ListTeachers")]
        public List<Teacher> ListTeachers()
        {
            // Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher>();

            // close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // open the connection
                Connection.Open();

                // Establish a new command for the database
                MySqlCommand Command = Connection.CreateCommand();

                // SQL query
                Command.CommandText = "SELECT * FROM teachers";

                // setting result set of query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Looping through each row in the result set
                    while (ResultSet.Read())
                    {
                        // access column information by the teacher table property/column name as an index
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                        // create Teacher object and populate properties
                        Teacher CurrentTeacher = new Teacher()
                        {
                            TeacherId = TeacherId,
                            TeacherFName = FirstName,
                            TeacherLName = LastName,
                            EmployeeNumber = EmployeeNumber,
                            HireDate = HireDate,
                            Salary = Salary
                        };

                        // add to the list of teachers
                        Teachers.Add(CurrentTeacher);
                    }
                }
            }

            // return the final list of teachers
            return Teachers;
        }



        /// <summary>
        /// returns a teacher in the database based on their id
        /// </summary>
        /// <example>
        /// GET: api/Teacher/FindTeacher/3 -> {"TeacherId":3,"TeacherFName":"Linda","TeacherLName":"Chan",...}
        /// </example>
        /// <param name="id">the id of the teacher to find</param>
        /// <returns>
        /// A matching teacher object by its id, will return a teacher object with null property values if teacher id is not in the database
        /// </returns>
        [HttpGet]
        [Route("FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
           

            // create an empty Teacher object
            Teacher SelectedTeacher = new Teacher();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // @id is replaced with the id value provided by user
                Command.CommandText = "SELECT * FROM teachers WHERE teacherid = @id";
                Command.Parameters.AddWithValue("@id", id);

                // gather Result Set of query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // loop through each row in the result set
                    if (ResultSet.Read())
                    {
                        // aceeess column information by the teacher table property/column name as an index
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                        SelectedTeacher.TeacherId = TeacherId;
                        SelectedTeacher.TeacherFName = FirstName;
                        SelectedTeacher.TeacherLName = LastName;
                        SelectedTeacher.EmployeeNumber = EmployeeNumber;
                        SelectedTeacher.HireDate = HireDate;
                        SelectedTeacher.Salary = Salary;
                    }

                
                }
            }

            // return the selected teacher
            return SelectedTeacher;
        }

        /// <summary>
        /// Uses input from a form to searchj for teachers in the database after providing a hire date range
        /// </summary>
        /// <example>
        /// GET: api/Teacher/SearchByHireDate?StartDate=2015-01-01&EndDate=2020-12-31 ->
        /// [{"TeacherId":1,"TeacherFName":"Alexander","TeacherLName":"Bennett",...},...]
        /// </example>
        /// <param name="StartDate">the start date of the hire range </param>
        /// <param name="EndDate">the end date of the hire range</param>
        /// <returns>
        /// A list of teacher objects hired within the specified date range.
        /// </returns>
        [HttpGet]
        [Route("SearchByHireDate")]
        public List<Teacher> SearchByHireDate(DateTime? StartDate = null, DateTime? EndDate = null)
        {
            // Create an empty list to hold Teacher objects
            List<Teacher> Teachers = new List<Teacher>();

            // Using statement ensures the connection is closed after execution
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection
                Connection.Open();

                // Create a new SQL command
                MySqlCommand Command = Connection.CreateCommand();

                // query to retrieve teachers
                string query = "SELECT * FROM teachers";

                // add criteria for hire date range if both startdate and endddate are provided
                if (StartDate.HasValue && EndDate.HasValue)
                {
                    query += " WHERE hiredate BETWEEN @startDate AND @endDate";
                    Command.Parameters.AddWithValue("@startDate", StartDate.Value.ToString("yyyy-MM-dd"));
                    Command.Parameters.AddWithValue("@endDate", EndDate.Value.ToString("yyyy-MM-dd"));
                }

                // assign the query to the command object
                Command.CommandText = query;
                Command.Prepare();

                // execute the query and store the result set
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // loop through each row in the result set
                    while (ResultSet.Read())
                    {
                        // Map each column to the properties of the Teacher object
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                        // create a new Teacher object and populate its properties
                        Teacher CurrentTeacher = new Teacher()
                        {
                            TeacherId = TeacherId,
                            TeacherFName = FirstName,
                            TeacherLName = LastName,
                            EmployeeNumber = EmployeeNumber,
                            HireDate = HireDate,
                            Salary = Salary
                        };

                        // add the Teacher object to the list
                        Teachers.Add(CurrentTeacher);
                    }
                }
            }

            // return the list of teachers
            return Teachers;
        }
    }
}
