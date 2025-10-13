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
    class Program
    {
        private static UniversitySystem _university = new UniversitySystem();

        static void Main(string[] args)
        {
            Console.WriteLine("University Management System");

            InitializeSampleData();

            bool exit = false;
            while (!exit)
            {
                DisplayMainMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageStudents();
                        break;
                    case "2":
                        ManageTeachers();
                        break;
                    case "3":
                        ManageCourses();
                        break;
                    case "4":
                        ViewAllData();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            Console.WriteLine("Thank you for using University Management System!");
        }
        static void DisplayMainMenu()
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Manage Students");
            Console.WriteLine("2. Manage Teachers");
            Console.WriteLine("3. Manage Courses");
            Console.WriteLine("4. View All Data");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
        }
        static void ManageStudents()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nStudent Management:");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. View All Students");
                Console.WriteLine("3. View Student Details");
                Console.WriteLine("4. Enroll Student in Course");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        Console.WriteLine(_university.GetAllStudentsInfo());
                        break;
                    case "3":
                        ViewStudentDetails();
                        break;
                    case "4":
                        EnrollStudentInCourse();
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void ManageTeachers()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nTeacher Management:");
                Console.WriteLine("1. Add Teacher");
                Console.WriteLine("2. View All Teachers");
                Console.WriteLine("3. View Teacher Details");
                Console.WriteLine("4. Assign Teacher to Course");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddTeacher();
                        break;
                    case "2":
                        Console.WriteLine(_university.GetAllTeachersInfo());
                        break;
                    case "3":
                        ViewTeacherDetails();
                        break;
                    case "4":
                        AssignTeacherToCourse();
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void ManageCourses()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nCourse Management:");
                Console.WriteLine("1. Add Course");
                Console.WriteLine("2. View All Courses");
                Console.WriteLine("3. View Course Details");
                Console.WriteLine("4. Back to Main Menu");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddCourse();
                        break;
                    case "2":
                        Console.WriteLine(_university.GetAllCoursesInfo());
                        break;
                    case "3":
                        ViewCourseDetails();
                        break;
                    case "4":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void AddStudent()
        {
            try
            {
                Console.Write("Enter student name: ");
                var name = Console.ReadLine();
                Console.Write("Enter age: ");
                var age = int.Parse(Console.ReadLine());
                Console.Write("Enter contact info: ");
                var contact = Console.ReadLine();
                Console.Write("Enter major: ");
                var major = Console.ReadLine();

                _university.AddStudent(name, age, contact, major);
                Console.WriteLine("Student added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void AddTeacher()
        {
            try
            {
                Console.Write("Enter teacher name: ");
                var name = Console.ReadLine();
                Console.Write("Enter age: ");
                var age = int.Parse(Console.ReadLine());
                Console.Write("Enter contact info: ");
                var contact = Console.ReadLine();
                Console.Write("Enter department: ");
                var department = Console.ReadLine();
                Console.Write("Enter specialization: ");
                var specialization = Console.ReadLine();

                _university.AddTeacher(name, age, contact, department, specialization);
                Console.WriteLine("Teacher added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void AddCourse()
        {
            try
            {
                Console.Write("Enter course name: ");
                var name = Console.ReadLine();
                Console.Write("Enter course description: ");
                var description = Console.ReadLine();
                Console.Write("Enter credits: ");
                var credits = int.Parse(Console.ReadLine());

                _university.AddCourse(name, description, credits);
                Console.WriteLine("Course added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ViewStudentDetails()
        {
            Console.Write("Enter student ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var student = _university.GetStudentById(id);
                if (student != null)
                {
                    Console.WriteLine(student.GetInfo());
                    Console.WriteLine(student.GetCoursesInfo());
                }
                else
                {
                    Console.WriteLine("Student not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        static void ViewTeacherDetails()
        {
            Console.Write("Enter teacher ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var teacher = _university.GetTeacherById(id);
                if (teacher != null)
                {
                    Console.WriteLine(teacher.GetInfo());
                    Console.WriteLine(teacher.GetTeachingCoursesInfo());
                }
                else
                {
                    Console.WriteLine("Teacher not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        static void ViewCourseDetails()
        {
            Console.Write("Enter course ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var course = _university.GetCourseById(id);
                if (course != null)
                {
                    Console.WriteLine(course.GetCourseInfo());
                    Console.WriteLine(course.GetStudentsList());
                }
                else
                {
                    Console.WriteLine("Course not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        static void EnrollStudentInCourse()
        {
            try
            {
                Console.Write("Enter student ID: ");
                var studentId = int.Parse(Console.ReadLine());
                Console.Write("Enter course ID: ");
                var courseId = int.Parse(Console.ReadLine());

                _university.EnrollStudentInCourse(studentId, courseId);
                Console.WriteLine("Student enrolled successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void AssignTeacherToCourse()
        {
            try
            {
                Console.Write("Enter teacher ID: ");
                var teacherId = int.Parse(Console.ReadLine());
                Console.Write("Enter course ID: ");
                var courseId = int.Parse(Console.ReadLine());

                _university.AssignTeacherToCourse(teacherId, courseId);
                Console.WriteLine("Teacher assigned successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ViewAllData()
        {
            Console.WriteLine("\n" + _university.GetAllStudentsInfo());
            Console.WriteLine(_university.GetAllTeachersInfo());
            Console.WriteLine(_university.GetAllCoursesInfo());
        }