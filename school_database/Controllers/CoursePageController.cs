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



        // GET: /CoursePage/NewCourse
        /// <summary>
        /// Displays a form to add a new course.
        /// </summary>
        /// <returns>A view with a form to add a course.</returns>
   
        [HttpGet]
		public IActionResult NewCourse()
		{
			return View();
		}


        /// <summary>
        /// Handles the submission of a new course form.
        /// </summary>
        /// <param name="NewCourse">A course object from the form.</param>
        /// <returns>Redirects to the page of the new course created.</returns>
		[HttpPost]
		public IActionResult Create(Course NewCourse)
		{
			int CourseId = _api.AddCourse(NewCourse);
			return RedirectToAction("Show", new { id = CourseId });
		}


        /// <summary>
        /// Displays a confirmation page for deleting a course.
        /// </summary>
        /// <param name="id">The ID of the course to delete.</param>
        /// <returns>A view asking for confirmation to delete selected course.</returns>
        [HttpGet]
		public IActionResult DeleteConfirm(int id)
		{
			Course SelectedCourse = _api.FindCourse(id); 
			return View(SelectedCourse);
		}


        /// <summary>
        /// Handles the deletion of a course.
        /// </summary>
        /// <param name="id">The ID of the course to delete.</param>
        /// <returns>Redirects to the list of courses.</returns>
        [HttpPost]
		public IActionResult Delete(int id)
		{
			_api.DeleteCourse(id);
			return RedirectToAction("List");
		}
	}
}