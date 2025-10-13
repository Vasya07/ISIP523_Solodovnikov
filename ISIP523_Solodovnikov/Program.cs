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
