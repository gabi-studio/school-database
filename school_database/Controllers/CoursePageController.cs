using Microsoft.AspNetCore.Mvc;
using School.Models;
using System.Collections.Generic;

namespace School.Controllers
{
    public class CoursePageController : Controller
    {
        // use the API to retrieve course information
        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }

        /// <summary>
        /// Retrieves a list of courses and passes it to the List view
        /// </summary>
        /// <returns>A view displaying the list of courses</returns>
        public IActionResult List()
        {
            // use the API to fetch a list of courses
            List<Course> Courses = _api.ListCourses();
            return View(Courses);
        }

        /// <summary>
        /// Retrieves a specific course by ID and passes it to the Show view
        /// </summary>
        /// <param name="id">The ID of the course to display</param>
        /// <returns>A view displaying the course details or an error view if the ID is invalid</returns>
        public IActionResult Show(int id)
        {
            // use the API to fetch a specific course by ID
            Course SelectedCourse = _api.FindCourse(id);

            // check if the course ID is valid
            if (SelectedCourse.CourseId <= 0)
            {
                // if not, pass an error message to the error view
                ViewBag.CourseError = "Course not found. Please check that you have the correct Course ID.";
                return View("CourseError");
            }
            else
            {
                // pass the Course object to the Show view
                return View(SelectedCourse);
            }
        }
    }
}