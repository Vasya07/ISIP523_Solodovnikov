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
            set => _name = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("ФИО не может быть пустым!");
        }

        public int Age
        {
            get => _age;
            set => _age = value >= 16 && value <= 100 ? value : throw new ArgumentException("Возраст должен быть от 16 до 100 лет!");
        }

        public string ContactInfo
        {
            get => _contactInfo;
            set => _contactInfo = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Адрес электронной почты не может быть пустым!");
        }

        public int Id
        {
            get => _id;
            private set => _id = value > 0 ? value : throw new ArgumentException("ID должен быть положительным!");
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
            set => _major = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Курс не может быть пустым!");
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
            sb.AppendLine($"ФИО: {Name}");
            sb.AppendLine($"ID: {Id}");
            sb.AppendLine($"Возраст : {Age}");
            sb.AppendLine($"На курсе: {Major}");
            sb.AppendLine($"Адрес электронной почты: {ContactInfo}");
            return sb.ToString();
        }

        public override string GetRole()
        {
            return "Student";
        }

        public string GetCoursesInfo()
        {
            if (_courses.Count == 0)
                return $"{Name} не записан ни на какие курсы!";

            var sb = new StringBuilder();
            sb.AppendLine($"Курсы для {Name}:");
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
            set => _department = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Курс не может быть пустым!");
        }

        public string Specialization
        {
            get => _specialization;
            set => _specialization = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Специализация не может быть пустой!");
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
            sb.AppendLine($"ФИО: {Name}");
            sb.AppendLine($"ID: {Id}");
            sb.AppendLine($"Возраст: {Age}");
            sb.AppendLine($"Курс: {Department}");
            sb.AppendLine($"Специализация: {Specialization}");
            sb.AppendLine($"Адрес электронной почты: {ContactInfo}");
            return sb.ToString();
        }

        public override string GetRole()
        {
            return "ФИО";
        }

        public string GetTeachingCoursesInfo()
        {
            if (_coursesTeaching.Count == 0)
                return $"{Name} не преподает никаких курсов";

            var sb = new StringBuilder();
            sb.AppendLine($"Курсы, проводимые {Name}:");
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
            set => _name = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Название курса не может быть пустым!");
        }

        public string Description
        {
            get => _description;
            set => _description = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Описание не может быть пустым!");
        }

        public int CourseId
        {
            get => _courseId;
            private set => _courseId = value > 0 ? value : throw new ArgumentException("ID курса должен быть положительным!");
        }

        public int Credits
        {
            get => _credits;
            set => _credits = value > 0 && value <= 10 ? value : throw new ArgumentException("Количесвто лет обучения на курсе должно быть от 1 до 10!");
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
            sb.AppendLine($"Курс: {Name}");
            sb.AppendLine($"ID: {CourseId}");
            sb.AppendLine($"Описание: {Description}");
            sb.AppendLine($"Количество лет обучения на курсе: {Credits}");
            sb.AppendLine($"Преподаватель: {(_teacher != null ? _teacher.Name : "Не назначен")}");
            sb.AppendLine($"Зачисленные студенты: {_students.Count}");
            return sb.ToString();
        }

        public string GetStudentsList()
        {
            if (_students.Count == 0)
                return $"Нет студентов, обучающихся на курсе {Name}.";

            var sb = new StringBuilder();
            sb.AppendLine($"Студенты, обучающиеся на курсе {Name}:");
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
                throw new ArgumentException("Студент не найден");
            if (course == null)
                throw new ArgumentException("Курс не найден");

            student.EnrollInCourse(course);
        }

        public void AssignTeacherToCourse(int teacherId, int courseId)
        {
            var teacher = GetTeacherById(teacherId);
            var course = GetCourseById(courseId);

            if (teacher == null)
                throw new ArgumentException("Преподаватель не найден");
            if (course == null)
                throw new ArgumentException("Курс не найден");

            teacher.AssignToCourse(course);
        }

        public IReadOnlyList<Student> GetAllStudents() => _students.AsReadOnly();
        public IReadOnlyList<Teacher> GetAllTeachers() => _teachers.AsReadOnly();
        public IReadOnlyList<Course> GetAllCourses() => _courses.AsReadOnly();

        public string GetAllStudentsInfo()
        {
            if (_students.Count == 0)
                return "В системе нет студентов";

            var sb = new StringBuilder();
            sb.AppendLine("Все студенты:");
            foreach (var student in _students)
            {
                sb.AppendLine(student.GetInfo());
            }
            return sb.ToString();
        }

        public string GetAllTeachersInfo()
        {
            if (_teachers.Count == 0)
                return "В системе нет учителей";

            var sb = new StringBuilder();
            sb.AppendLine("Все преподаватели:");
            foreach (var teacher in _teachers)
            {
                sb.AppendLine(teacher.GetInfo());
            }
            return sb.ToString();
        }

        public string GetAllCoursesInfo()
        {
            if (_courses.Count == 0)
                return "В системе нет курсов";

            var sb = new StringBuilder();
            sb.AppendLine("Все курсы:");
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
            Console.Clear();
            Console.WriteLine("Система управления университетом");

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
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }
        static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("\nГлавное меню:");
            Console.WriteLine("1. Управление студентами");
            Console.WriteLine("2. Управление учителями");
            Console.WriteLine("3. Управление курсами");
            Console.WriteLine("4. Просмотреть всё возможное и невозможное");
            Console.WriteLine("5. Выход");
            Console.Write("Ваш выбор: ");
        }
        static void ManageStudents()
        {
            Console.Clear();
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nУправление студентами:");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Просмотреть всех студентов");
                Console.WriteLine("3. Просмотр сведений о студенте");
                Console.WriteLine("4. Зачислить студента на курс");
                Console.WriteLine("5. Назад в главное меню");
                Console.Write("Ваш выбор: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        AddStudent();
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine(_university.GetAllStudentsInfo());
                        break;
                    case "3":
                        Console.Clear();
                        ViewStudentDetails();
                        break;
                    case "4":
                        Console.Clear();
                        EnrollStudentInCourse();
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }

        static void ManageTeachers()
        {
            Console.Clear();
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nУправление преподавателями:");
                Console.WriteLine("1. Добавить преподавателя");
                Console.WriteLine("2. Просмотреть всех преподавателей");
                Console.WriteLine("3. Просмотр сведений о преподавателе");
                Console.WriteLine("4. Назначить преподавателя на курс");
                Console.WriteLine("5. Назад в главное меню");
                Console.Write("Ваш выбор: ");

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
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }

        static void ManageCourses()
        {
            Console.Clear();
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nУправление курсами:");
                Console.WriteLine("1. Добавить курс");
                Console.WriteLine("2. Просмотреть все курсы");
                Console.WriteLine("3. Просмотреть детали курса");
                Console.WriteLine("4. Назад в главное меню");
                Console.Write("Ваш выбор: ");

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
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }

        static void AddStudent()
        {
            Console.Clear();
            try
            {
                Console.Write("Введите ФИО студента: ");
                var name = Console.ReadLine();
                Console.Write("Введите возраст студента: ");
                var age = int.Parse(Console.ReadLine());
                Console.Write("Введите адрес элекронной почты: ");
                var contact = Console.ReadLine();
                Console.Write("Введите название предмета: ");
                var major = Console.ReadLine();

                _university.AddStudent(name, age, contact, major);
                Console.WriteLine("Студент успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА: {ex.Message}");
            }
        }

        static void AddTeacher()
        {
            Console.Clear();
            try
            {
                Console.Write("Введите ФИО преподавателя: ");
                var name = Console.ReadLine();
                Console.Write("Введите возраст преподавателя: ");
                var age = int.Parse(Console.ReadLine());
                Console.Write("Введите адрес элекронной почты: ");
                var contact = Console.ReadLine();
                Console.Write("Введите название предмета: ");
                var department = Console.ReadLine();
                Console.Write("Введите специализацию: ");
                var specialization = Console.ReadLine();

                _university.AddTeacher(name, age, contact, department, specialization);
                Console.WriteLine("Учитель успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА: {ex.Message}");
            }
        }

        static void AddCourse()
        {
            Console.Clear();
            try
            {
                Console.Write("Введите название курса: ");
                var name = Console.ReadLine();
                Console.Write("Введите описание курса: ");
                var description = Console.ReadLine();
                Console.Write("Введите количество лет обучения на курсе: ");
                var credits = int.Parse(Console.ReadLine());

                _university.AddCourse(name, description, credits);
                Console.WriteLine("Курс успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА: {ex.Message}");
            }
        }

        static void ViewStudentDetails()
        {
            Console.Clear();
            Console.Write("Введите ID студента: ");
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
                    Console.WriteLine("Студент не найден!");
                }
            }
            else
            {
                Console.WriteLine("Неверный ID!");
            }
        }

        static void ViewTeacherDetails()
        {
            Console.Clear();
            Console.Write("Введите ID преподавателя: ");
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
                    Console.WriteLine("Преподаватель не найден!");
                }
            }
            else
            {
                Console.WriteLine("Неверный ID!");
            }
        }

        static void ViewCourseDetails()
        {
            Console.Clear();
            Console.Write("Введите ID курса: ");
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
                    Console.WriteLine("Курс не найден!");
                }
            }
            else
            {
                Console.WriteLine("Неверный ID!");
            }
        }

        static void EnrollStudentInCourse()
        {
            Console.Clear();
            try
            {
                Console.Write("Введите ID студента: ");
                var studentId = int.Parse(Console.ReadLine());
                Console.Write("Введите ID курса: ");
                var courseId = int.Parse(Console.ReadLine());

                _university.EnrollStudentInCourse(studentId, courseId);
                Console.WriteLine("Студент успешно зачислен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА: {ex.Message}");
            }
        }

        static void AssignTeacherToCourse()
        {
            Console.Clear();
            try
            {
                Console.Write("Введите ID преподавателя: ");
                var teacherId = int.Parse(Console.ReadLine());
                Console.Write("Введите ID курса: ");
                var courseId = int.Parse(Console.ReadLine());

                _university.AssignTeacherToCourse(teacherId, courseId);
                Console.WriteLine("Преподаватель успешно назначен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА: {ex.Message}");
            }
        }

        static void ViewAllData()
        {
            Console.WriteLine("\n" + _university.GetAllStudentsInfo());
            Console.WriteLine(_university.GetAllTeachersInfo());
            Console.WriteLine(_university.GetAllCoursesInfo());
            Console.WriteLine("Нажмите любую клавишу для продолжения");
            Console.ReadKey();
        }
        static void InitializeSampleData()
        {
            _university.AddStudent("Солодовников Василий Вячеславович", 18, "vaska@xmail.ru", "Программирование на C#");
            _university.AddStudent("Кураев Даниил (to be filled by O.E.M)", 19, "daniel@xmail.com", "Анализ Big Data на Python");
            _university.AddTeacher("Гордов Максим Олегович", 37, "maks@edu.fa.ru", "Программирование на C#", "Программирование");
            _university.AddTeacher("Горланов Владимир Владимирович", 36, "vladimir@edu.fa.ru", "Анализ Big Data на Python", "Анализ данных");
            _university.AddCourse("Welcome To The C#", "Базовые концепции программирования", 3);
            _university.AddCourse("Telemetry on Python", "Базовые концепции анализа Big Data", 4);

            _university.AssignTeacherToCourse(1, 1);
            _university.AssignTeacherToCourse(2, 2);
            _university.EnrollStudentInCourse(1, 1);
            _university.EnrollStudentInCourse(2, 1);
            _university.EnrollStudentInCourse(2, 2);
        }
    }
}