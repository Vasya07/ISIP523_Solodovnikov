using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversityManagement
{
    public abstract class Person
    {
        private string _name;
        private int _age;
        private string _contactInfo;
        private int _id;

        public Person(string name, int age, string contactInfo, int id)
        {
            Name = name;
            Age = age;
            ContactInfo = contactInfo;
            Id = id;
        }

        public string Name
        {
            get => _name;
            set => _name = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Name cannot be empty");
        }

        public int Age
        {
            get => _age;
            set => _age = value >= 16 && value <= 100 ? value : throw new ArgumentException("Age must be between 16 and 100");
        }

        public string ContactInfo
        {
            get => _contactInfo;
            set => _contactInfo = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Contact info cannot be empty");
        }

        public int Id
        {
            get => _id;
            private set => _id = value > 0 ? value : throw new ArgumentException("ID must be positive");
        }

        public abstract string GetInfo();
        public abstract string GetRole();
    }
    public class Student : Person
    {
        private string _major;
        private List<Course> _courses;

        public Student(string name, int age, string contactInfo, int id, string major)
            : base(name, age, contactInfo, id)
        {
            Major = major;
            _courses = new List<Course>();
        }

        public string Major
        {
            get => _major;
            set => _major = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Major cannot be empty");
        }

        public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

        public void EnrollInCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (!_courses.Contains(course))
            {
                _courses.Add(course);
                course.AddStudent(this);
            }
        }

        public override string GetInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Student: {Name}");
            sb.AppendLine($"ID: {Id}");
            sb.AppendLine($"Age: {Age}");
            sb.AppendLine($"Major: {Major}");
            sb.AppendLine($"Contact: {ContactInfo}");
            return sb.ToString();
        }

        public override string GetRole()
        {
            return "Student";
        }

        public string GetCoursesInfo()
        {
            if (_courses.Count == 0)
                return $"{Name} is not enrolled in any courses.";

            var sb = new StringBuilder();
            sb.AppendLine($"Courses for {Name}:");
            foreach (var course in _courses)
            {
                sb.AppendLine($"- {course.Name} (ID: {course.CourseId})");
            }
            return sb.ToString();
        }
    }
    public class Teacher : Person
    {
        private string _department;
        private string _specialization;
        private List<Course> _coursesTeaching;

        public Teacher(string name, int age, string contactInfo, int id, string department, string specialization)
            : base(name, age, contactInfo, id)
        {
            Department = department;
            Specialization = specialization;
            _coursesTeaching = new List<Course>();
        }

        public string Department
        {
            get => _department;
            set => _department = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Department cannot be empty");
        }

        public string Specialization
        {
            get => _specialization;
            set => _specialization = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Specialization cannot be empty");
        }

        public IReadOnlyList<Course> CoursesTeaching => _coursesTeaching.AsReadOnly();

        public void AssignToCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (!_coursesTeaching.Contains(course))
            {
                _coursesTeaching.Add(course);
                course.AssignTeacher(this);
            }
        }

        public override string GetInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Professor: {Name}");
            sb.AppendLine($"ID: {Id}");
            sb.AppendLine($"Age: {Age}");
            sb.AppendLine($"Department: {Department}");
            sb.AppendLine($"Specialization: {Specialization}");
            sb.AppendLine($"Contact: {ContactInfo}");
            return sb.ToString();
        }

        public override string GetRole()
        {
            return "Professor";
        }

        public string GetTeachingCoursesInfo()
        {
            if (_coursesTeaching.Count == 0)
                return $"{Name} is not teaching any courses.";

            var sb = new StringBuilder();
            sb.AppendLine($"Courses taught by {Name}:");
            foreach (var course in _coursesTeaching)
            {
                sb.AppendLine($"- {course.Name} (ID: {course.CourseId})");
            }
            return sb.ToString();
        }
    }
    public class Course
    {
        private string _name;
        private string _description;
        private int _courseId;
        private int _credits;
        private Teacher _teacher;
        private List<Student> _students;

        public Course(string name, string description, int courseId, int credits)
        {
            Name = name;
            Description = description;
            CourseId = courseId;
            Credits = credits;
            _students = new List<Student>();
        }

        public string Name
        {
            get => _name;
            set => _name = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Course name cannot be empty");
        }

        public string Description
        {
            get => _description;
            set => _description = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Description cannot be empty");
        }

        public int CourseId
        {
            get => _courseId;
            private set => _courseId = value > 0 ? value : throw new ArgumentException("Course ID must be positive");
        }

        public int Credits
        {
            get => _credits;
            set => _credits = value > 0 && value <= 10 ? value : throw new ArgumentException("Credits must be between 1 and 10");
        }

        public Teacher Teacher => _teacher;
        public IReadOnlyList<Student> Students => _students.AsReadOnly();

        public void AssignTeacher(Teacher teacher)
        {
            _teacher = teacher;
        }

        public void AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (!_students.Contains(student))
            {
                _students.Add(student);
            }
        }

        public string GetCourseInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Course: {Name}");
            sb.AppendLine($"ID: {CourseId}");
            sb.AppendLine($"Description: {Description}");
            sb.AppendLine($"Credits: {Credits}");
            sb.AppendLine($"Teacher: {(_teacher != null ? _teacher.Name : "Not assigned")}");
            sb.AppendLine($"Enrolled students: {_students.Count}");
            return sb.ToString();
        }

        public string GetStudentsList()
        {
            if (_students.Count == 0)
                return $"No students enrolled in {Name}.";

            var sb = new StringBuilder();
            sb.AppendLine($"Students enrolled in {Name}:");
            foreach (var student in _students)
            {
                sb.AppendLine($"- {student.Name} (ID: {student.Id})");
            }
            return sb.ToString();
        }
    }
    public class UniversitySystem
    {
        private List<Student> _students;
        private List<Teacher> _teachers;
        private List<Course> _courses;
        private int _nextStudentId;
        private int _nextTeacherId;
        private int _nextCourseId;

        public UniversitySystem()
        {
            _students = new List<Student>();
            _teachers = new List<Teacher>();
            _courses = new List<Course>();
            _nextStudentId = 1;
            _nextTeacherId = 1;
            _nextCourseId = 1;
        }

        public void AddStudent(string name, int age, string contactInfo, string major)
        {
            var student = new Student(name, age, contactInfo, _nextStudentId++, major);
            _students.Add(student);
        }

        public void AddTeacher(string name, int age, string contactInfo, string department, string specialization)
        {
            var teacher = new Teacher(name, age, contactInfo, _nextTeacherId++, department, specialization);
            _teachers.Add(teacher);
        }

        public void AddCourse(string name, string description, int credits)
        {
            var course = new Course(name, description, _nextCourseId++, credits);
            _courses.Add(course);
        }

        public Student GetStudentById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }

        public Teacher GetTeacherById(int id)
        {
            return _teachers.FirstOrDefault(t => t.Id == id);
        }

        public Course GetCourseById(int id)
        {
            return _courses.FirstOrDefault(c => c.CourseId == id);
        }

        public void EnrollStudentInCourse(int studentId, int courseId)
        {
            var student = GetStudentById(studentId);
            var course = GetCourseById(courseId);

            if (student == null)
                throw new ArgumentException("Student not found");
            if (course == null)
                throw new ArgumentException("Course not found");

            student.EnrollInCourse(course);
        }

        public void AssignTeacherToCourse(int teacherId, int courseId)
        {
            var teacher = GetTeacherById(teacherId);
            var course = GetCourseById(courseId);

            if (teacher == null)
                throw new ArgumentException("Teacher not found");
            if (course == null)
                throw new ArgumentException("Course not found");

            teacher.AssignToCourse(course);
        }

        public IReadOnlyList<Student> GetAllStudents() => _students.AsReadOnly();
        public IReadOnlyList<Teacher> GetAllTeachers() => _teachers.AsReadOnly();
        public IReadOnlyList<Course> GetAllCourses() => _courses.AsReadOnly();

        public string GetAllStudentsInfo()
        {
            if (_students.Count == 0)
                return "No students in the system.";

            var sb = new StringBuilder();
            sb.AppendLine("All Students:");
            foreach (var student in _students)
            {
                sb.AppendLine(student.GetInfo());
            }
            return sb.ToString();
        }

        public string GetAllTeachersInfo()
        {
            if (_teachers.Count == 0)
                return "No teachers in the system.";

            var sb = new StringBuilder();
            sb.AppendLine("All Teachers:");
            foreach (var teacher in _teachers)
            {
                sb.AppendLine(teacher.GetInfo());
            }
            return sb.ToString();
        }

        public string GetAllCoursesInfo()
        {
            if (_courses.Count == 0)
                return "No courses in the system.";

            var sb = new StringBuilder();
            sb.AppendLine("All Courses:");
            foreach (var course in _courses)
            {
                sb.AppendLine(course.GetCourseInfo());
            }
            return sb.ToString();
        }
    }
