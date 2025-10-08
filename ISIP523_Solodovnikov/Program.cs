namespace BAM
{
    public class BankAccount
    {
        private static int _accountCounter = 1;
        private readonly int _accountNumber;
        private string _ownerName;
        private decimal _balance;
        private decimal _operationLimit;
        private bool _isActive;

        public BankAccount(string ownerName, decimal initialBalance = 0, decimal operationLimit = 10000)
        {
            _accountNumber = GenerateAccountNumber();
            OwnerName = ownerName;
            _balance = initialBalance;
            OperationLimit = operationLimit;
            _isActive = true;
        }

        public int AccountNumber => _accountNumber;

        public string OwnerName
        {
            get => _ownerName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Имя владельца не может быть пустым!");
                _ownerName = value.Trim();
            }
        }

        public decimal Balance => _balance;

        public decimal OperationLimit
        {
            get => _operationLimit;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Лимит операции не может быть отрицательным!");
                _operationLimit = value;
            }
        }

        public bool IsActive => _isActive;
        public string Status => _isActive ? "Активен" : "Закрыт";

        private static int GenerateAccountNumber()
        {
            return _accountCounter++;
        }

        public void Deposit(decimal amount)
        {
            ValidateAccountActive();
            if (amount <= 0)
                throw new ArgumentException("Сумма пополнения должна быть положительной!");

            _balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            ValidateAccountActive();

            if (amount <= 0)
                throw new ArgumentException("Сумма списания должна быть положительной!");

            if (amount > _balance)
                throw new InvalidOperationException("Недостаточно средств на счёте!");

            if (amount > _operationLimit)
                throw new InvalidOperationException($"Сумма списания превышает лимит операции ({_operationLimit:C})!");

            _balance -= amount;
        }

        public void ChangeOperationLimit(decimal newLimit)
        {
            ValidateAccountActive();
            OperationLimit = newLimit;
        }

        public void ChangeOwnerName(string newName)
        {
            ValidateAccountActive();
            OwnerName = newName;
        }

        public void ShowAccountInfo()
        {
            Console.WriteLine("Информация о счёте");
            Console.WriteLine($"Номер счёта: {_accountNumber}");
            Console.WriteLine($"Владелец: {_ownerName}");
            Console.WriteLine($"Баланс: {_balance:C}");
            Console.WriteLine($"Лимит операции: {_operationLimit:C}");
            Console.WriteLine($"Статус: {Status}");
        }

        public void CloseAccount()
        {
            if (!_isActive)
                throw new InvalidOperationException("ОШИБКА: Счёт уже закрыт!");

            _isActive = false;
        }

        private void ValidateAccountActive()
        {
            if (!_isActive)
                throw new InvalidOperationException("ОПЕРАЦИЯ НЕВОЗМОЖНА: счёт закрыт!");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            BankAccount account = null;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("1 - Открыть счёт");
                Console.WriteLine("2 - Пополнить счёт");
                Console.WriteLine("3 - Снять со счёта");
                Console.WriteLine("4 - Изменить лимит операции");
                Console.WriteLine("5 - Изменить имя владельца");
                Console.WriteLine("6 - Показать информацию о счёте");
                Console.WriteLine("7 - Закрыть счёт");
                Console.WriteLine("0 - Выход");

                Console.Write("\nВыберите команду: ");
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            account = CreateAccount();
                            break;
                        case "2":
                            DepositToAccount(account);
                            break;
                        case "3":
                            WithdrawFromAccount(account);
                            break;
                        case "4":
                            ChangeOperationLimit(account);
                            break;
                        case "5":
                            ChangeOwnerName(account);
                            break;
                        case "6":
                            ShowAccountInfo(account);
                            break;
                        case "7":
                            CloseAccount(account);
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Недопустимая команда!");
                            WaitForKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ОШИБКА: {ex.Message}");
                    WaitForKey();
                }
            }
        }
        static BankAccount CreateAccount()
        {
            Console.Clear();

            string ownerName;
            while (true)
            {
                Console.Write("Введите имя владельца: ");
                ownerName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(ownerName))
                {
                    Console.WriteLine("ОШИБКА: Имя владельца не может быть пустым!");
                }
                else
                {
                    break;
                }
            }

            decimal initialBalance;
            while (true)
            {
                try
                {
                    Console.Write("Введите начальный баланс: ");
                    initialBalance = Convert.ToInt32((Console.ReadLine()));

                    if (initialBalance < 0)
                    {
                        Console.WriteLine("ОШИБКА: Начальный баланс не может быть отрицательным!");
                    }
                    else
                    {
                        break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("ОШИБКА: Неверно введён начальный баланс! Используйте числа.");
                }
            }

            decimal operationLimit;
            while (true)
            {
                try
                {
                    Console.Write("Введите лимит операции: ");
                    operationLimit = Convert.ToInt32((Console.ReadLine()));

                    if (operationLimit < 0)
                    {
                        Console.WriteLine("ОШИБКА: Лимит операции не может быть отрицательным!");
                    }
                    else
                    {
                        break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("ОШИБКА: Неверно введён лимит операции! Используйте числа.");
                }
            }

            var account = new BankAccount(ownerName, initialBalance, operationLimit);
            Console.WriteLine("\nСчёт успешно создан!");
            Console.WriteLine($"Номер счёта: {account.AccountNumber}");
            WaitForKey();
            return account;
        }