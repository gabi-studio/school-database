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


        // GET: /StudentPage/NewStudent
        /// <summary>
        /// Displays a form to add a new student.
        /// </summary>
        /// <returns>A view with a form to add a student.</returns>
        [HttpGet]
        public IActionResult NewStudent()
        {
            return View();
        }


        /// <summary>
        /// Handles the submission of a new student form.
        /// </summary>
        /// <param name="NewStudent">A student object from the form.</param>
        /// <returns>Redirects to the page of the new student created.</returns>
        [HttpPost]
        public IActionResult Create(Student NewStudent)
        {
            int StudentId = _api.AddStudent(NewStudent);
            return RedirectToAction("Show", new { id = StudentId });
        }

        /// <summary>
        /// Displays a confirmation page for deleting a student.
        /// </summary>
        /// <param name="id">The ID of the student to delete.</param>
        /// <returns>A view asking for confirmation to delete selected student.</returns>
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Student SelectedStudent = _api.FindStudent(id); // assumign student exists
            return View(SelectedStudent);
        }

        /// <summary>
        /// Handles the deletion of a student.
        /// </summary>
        /// <param name="id">The ID of the student to delete.</param>
        /// <returns>Redirects to the list of students.</returns>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _api.DeleteStudent(id);
            return RedirectToAction("List");
        }

		public IActionResult Edit(int id)
		{
			Student SelectedStudent = _api.FindStudent(id);
			if (SelectedStudent.StudentId == 0)
			{
				ViewBag.ErrorMessage = "Student not found. Please check the ID.";
				return View("StudentError");
			}
			return View(SelectedStudent);
		}

		[HttpPost]
		public IActionResult Update(int id, string StudentFName, string StudentLName, string StudentNumber, DateTime EnrollDate)
		{
			// if student name is empty
			if (string.IsNullOrEmpty(StudentFName) || string.IsNullOrEmpty(StudentLName))
			{
				ViewBag.ClientError = "Please fill student name.";
                return Edit(id);
            }
			

			Student UpdatedStudent = new Student
			{
				StudentFName = StudentFName,
				StudentLName = StudentLName,
				StudentNumber = StudentNumber,
				EnrollDate = EnrollDate
			};

			_api.UpdateStudent(id, UpdatedStudent);
			return RedirectToAction("Show", new { id = id });
		}


	}
}