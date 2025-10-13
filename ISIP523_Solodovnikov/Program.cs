using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversityManagement
{
    // Базовый абстрактный класс Person
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
}