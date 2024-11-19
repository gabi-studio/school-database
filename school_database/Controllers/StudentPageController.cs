using Microsoft.AspNetCore.Mvc;
using School.Models;
using System.Collections.Generic;

namespace School.Controllers
{
    public class StudentPageController : Controller
    {
        // Use the API to retrieve student information
        private readonly StudentAPIController _api;

        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }

        /// <summary>
        /// Retrieves a list of students and passes it to the List view
        /// </summary>
        /// <returns>A view displaying the list of students</returns>
        public IActionResult List()
        {
            // Use the API to fetch a list of students
            List<Student> Students = _api.ListStudents();
            return View(Students);
        }

        /// <summary>
        /// Retrieves a specific student by ID and passes it to the Show view
        /// </summary>
        /// <param name="id">The ID of the student to display</param>
        /// <returns>A view displaying the student details or an error view if the ID is invalid</returns>
        public IActionResult Show(int id)
        {
            // Use the API to fetch a specific student by ID
            Student SelectedStudent = _api.FindStudent(id);

            // Check if the student ID is not valid
            if (SelectedStudent.StudentId <= 0)
            {
                // if not, pass an error message to the error view
                ViewBag.StudentError = "Student not found. Please check that you have the correct Student ID.";
                return View("StudentError");
            }
            else
            {
                // otherwise pass the Student object to the Show view
                return View(SelectedStudent);
            }
        }
    }
}