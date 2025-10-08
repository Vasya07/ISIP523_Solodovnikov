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