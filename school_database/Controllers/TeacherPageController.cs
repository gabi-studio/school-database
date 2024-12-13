using Microsoft.AspNetCore.Mvc;
using School.Models;
using System.Collections.Generic;

namespace School.Controllers
{
    public class TeacherPageController : Controller
    {
        // use the API to retrieve teacher information
    
        private readonly TeacherAPIController _api;

        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }

        // GET: TeacherPage/List
        public IActionResult List()
        {
            // use the API to retrieve a list of teachers
            List<Teacher> Teachers = _api.ListTeachers();
            return View(Teachers);
        }

        // GET: TeacherPage/Show/{id}
        public IActionResult Show(int id)
        {
            // use the API to retrieve a specific teacher by id
            Teacher SelectedTeacher = _api.FindTeacher(id);

            // check if the teacher id of the selected teacher is valid
            if (SelectedTeacher.TeacherId <= 0)
            {
                // if not, pass an error message to the error view
                ViewBag.TeacherError = "Teacher not found. Please check that you have the correct Teacher ID.";
                return View("TeacherError");
            }

            // // otherwise pass the Teacher object to the Show view
            else
            {
                return View(SelectedTeacher);
            }
            
        }

        // GET: api/Teacher/SearchByHireDate?StartDate=2015-01-01&EndDate=2020-12-31
        public IActionResult SearchByHireDate(DateTime? startDate, DateTime? endDate)
        {
            // Use the API to search for teachers by hire date within the specified range
            List<Teacher> Teachers = _api.SearchByHireDate(startDate, endDate);

            // Pass the results to the List view
            return View("List", Teachers);
        }

        // GET: /TeacherPage/NewTeacher
        /// <summary>
        /// Displays a form to add a new teacher.
        /// </summary>
        /// <returns>A view with a form to add a teacher.</returns>
        [HttpGet]
        public IActionResult NewTeacher()
        {
            return View("NewTeacher");
        }

        /// <summary>
        /// Handles the submission of a new teacher form.
        /// </summary>
        /// <param name="NewTeacher">A Teacher object from the form.</param>
        /// <returns>Redirects to the list of teachers.</returns>
        [HttpPost]
        public IActionResult Create(Teacher NewTeacher)
        {
            int TeacherId = _api.AddTeacher(NewTeacher);
            return RedirectToAction("Show", new { id = TeacherId });
        }

        /// <summary>
        /// Displays a confirmation page for deleting a teacher.
        /// </summary>
        /// <param name="id">The ID of the teacher to delete.</param>
        /// <returns>A view asking for confirmation to delete selected teacher.</returns>
        public IActionResult DeleteConfirm(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }

        /// <summary>
        /// Handles the deletion of a teacher.
        /// </summary>
        /// <param name="id">The ID of the teacher to delete.</param>
        /// <returns>Redirects to the list of teachers.</returns>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _api.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        /// <summary>
        /// Displays a form to edit a teacher's information.
        /// </summary>
        /// <param name="id">The ID of the teacher to edit.</param>
        /// <example>
        /// GET: /TeacherPage/Edit/1
        /// </example>
        /// <returns>A view with a form pre-filled with the teacher's information.</returns>
        public IActionResult Edit(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            if (SelectedTeacher == null || SelectedTeacher.TeacherId <= 0)
            {
                ViewBag.ErrorMessage = "Teacher not found. Please check that you entered the correct ID.";
                return RedirectToAction("List");
            }
            return View(SelectedTeacher);
        }

        /// <summary>
        /// updates a teacher's information based on form submission.
        /// </summary>
        /// <param name="id">The ID of the teacher to update.</param>
        /// <param name="TeacherFName">The updated first name of the teacher.</param>
        /// <param name="TeacherLName">The updated last name of the teacher.</param>
        /// <param name="EmployeeNumber">The updated employee number of the teacher.</param>
        /// <param name="HireDate">The updated hire date of the teacher.</param>
        /// <param name="Salary">The updated salary of the teacher.</param>
        /// <example>
        /// POST: /TeacherPage/Edit/1
        /// </example>
        /// <returns>Redirects to the Show view for the updated teacher or displays validation errors.</returns>
        [HttpPost]
        public IActionResult Update(int id, string TeacherFName, string TeacherLName, string EmployeeNumber, DateTime HireDate, decimal Salary)
        {
            // Error message for if teacher name is empty
            if (string.IsNullOrEmpty(TeacherFName) || string.IsNullOrEmpty(TeacherLName))
            {
                ViewBag.ClientError = "Please fill the teacher's name.";
                // Return the Edit view with the current teacher's data and error message
                return View("Edit", new Teacher
                {
                    TeacherId = id,
                    TeacherFName = TeacherFName,
                    TeacherLName = TeacherLName,
                    EmployeeNumber = EmployeeNumber,
                    HireDate = HireDate,
                    Salary = Salary
                });
            }

            // Error message for if hire date is in the future
            if (HireDate > DateTime.Now)
            {
                ViewBag.ClientError = "Hire date cannot be in the future.";
                return View("Edit", new Teacher
                {
                    TeacherId = id,
                    TeacherFName = TeacherFName,
                    TeacherLName = TeacherLName,
                    EmployeeNumber = EmployeeNumber,
                    HireDate = HireDate,
                    Salary = Salary
                });
            }

            // Error message for if salary is less than zero
            if (Salary < 0)
            {
                ViewBag.ClientError = "Please fill salary with the correct value.";
                return View("Edit", new Teacher
                {
                    TeacherId = id,
                    TeacherFName = TeacherFName,
                    TeacherLName = TeacherLName,
                    EmployeeNumber = EmployeeNumber,
                    HireDate = HireDate,
                    Salary = Salary
                });
            }

            // If validation passes, proceed to update teacher
            Teacher UpdatedTeacher = new Teacher
            {
                TeacherId = id,
                TeacherFName = TeacherFName,
                TeacherLName = TeacherLName,
                EmployeeNumber = EmployeeNumber,
                HireDate = HireDate,
                Salary = Salary
            };

            _api.UpdateTeacher(id, UpdatedTeacher);

            // Redirect to the Show page after successful update
            return RedirectToAction("Show", new { id = id });
        }



    }
}
