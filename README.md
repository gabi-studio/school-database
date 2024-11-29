# **School Management System**

## **Overview**

This is a basic project for a **School Management System** built using **ASP.NET Core**. 
The current version only has the **Read**,  **Create**,  and **Delete** functionalities for managing Teachers, Students, and Courses, and the **Update** feature will be added in future versions.


This project connects to a **MySQL database** and uses **ASP.NET Core MVC** for the web pages and **Web API** for the backend.

---

## **Features (Current Version)**

### **Teachers**
- View a list of all teachers.
- Search for teachers by their hire date within a range.
- View detailed information for a specific teacher.
- Show an error message if the teacher ID is invalid.
- Use a form to add a new teacher
- Delete a teacher (with a confirmation page to safeguard delete)

### **Students**
- View a list of all students.
- View details of a specific student.
- Show an error message if the student ID is invalid.
- Use a form to add a new student
- Delete a student (with a confirmation page to safeguard delete)

### **Courses**
- View a list of all courses.
- View details of a specific course.
- Show an error message if the course ID is invalid.
- Use a form to add a new course
- Delete a course (with a confirmation page to safeguard delete)

---

## **What Will Be Added in the Future**
- **Create**: ~~Add new Teachers, Students, and Courses.~~ (Added 2024/11/29)
- **Update**: Edit existing records in the database.
- **Delete**: ~~Remove records from the database.~~ (Added 2024/11/29)


## **API Endpoints**

### **Teachers**
- `GET /api/Teacher/ListTeachers`: Gets all teachers.
- `GET /api/Teacher/FindTeacher/{id}`: Finds a teacher by ID.
- `GET /api/Teacher/SearchByHireDate?StartDate={StartDate}&EndDate={EndDate}`: Searches teachers hired between two dates.
- `GET /api/Teacher/AddTeacher`: Adds a new teacher
- `GET /api/Teacher/DeleteTeacher/{id}`: Deletes a teacher by ID.

### **Students**
- `GET /api/Student/ListStudents`: Gets all students.
- `GET /api/Student/FindStudent/{id}`: Finds a student by ID.
- `GET /api/Student/AddStudent`: Adds a new student
- `GET /api/STudent/DeleteStudent/{id}`: Deletes a student by ID.

### **Courses**
- `GET /api/Course/ListCourses`: Gets all courses.
- `GET /api/Course/FindCourse/{id}`: Finds a course by ID.
- `GET /api/Course/AddCourse`: Adds a new course
- `GET /api/Course/DeleteCourse/{id}`: Deletes a course by ID.
