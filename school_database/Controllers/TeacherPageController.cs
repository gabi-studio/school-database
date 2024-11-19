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

    }
}
