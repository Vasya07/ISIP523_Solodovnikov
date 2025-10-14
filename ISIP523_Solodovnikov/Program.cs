namespace TextRoguelike
{
    public abstract class Item
    {
        public string Name { get; protected set; }

        public Item(string name)
        {
            Name = name;
        }

        public abstract void DisplayStats();
    }

    public class Weapon : Item
    {
        public int Attack { get; private set; }

        public Weapon(string name, int attack) : base(name)
        {
            Attack = attack;
        }

        public override void DisplayStats()
        {
            Console.WriteLine($"{Name} (Атака: {Attack})");
        }
    }

    public class Armor : Item
    {
        public int Defense { get; private set; }

        public Armor(string name, int defense) : base(name)
        {
            Defense = defense;
        }

        public override void DisplayStats()
        {
            Console.WriteLine($"{Name} (Защита: {Defense})");
        }
    }
    public abstract class Creature
    {
        public string Name { get; protected set; }
        public int MaxHP { get; protected set; }
        public int CurrentHP { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }

        public bool IsAlive => CurrentHP > 0;

        protected Creature(string name, int maxHP, int attack, int defense)
        {
            Name = name;
            MaxHP = maxHP;
            CurrentHP = maxHP;
            Attack = attack;
            Defense = defense;
        }

        public virtual void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP < 0) CurrentHP = 0;
        }

        public void Heal(int amount)
        {
            CurrentHP += amount;
            if (CurrentHP > MaxHP) CurrentHP = MaxHP;
        }

        public virtual void DisplayStats()
        {
            Console.WriteLine($"{Name} - HP: {CurrentHP}/{MaxHP}, Атака: {Attack}, Защита: {Defense}");
        }
    }
    public abstract class Enemy : Creature
    {
        protected Random random;

        public Enemy(string name, int maxHP, int attack, int defense) : base(name, maxHP, attack, defense)
        {
            random = new Random();
        }

        public abstract int CalculateDamage(int playerDefense);
        public abstract string GetSpecialAbility();
    }
    public class Goblin : Enemy
    {
        private const double CRIT_CHANCE = 0.2;
        private const double CRIT_MULTIPLIER = 1.5;

        public Goblin() : base("Гоблин", 30, 8, 3) { }

        public override int CalculateDamage(int playerDefense)
        {
            int baseDamage = Math.Max(1, Attack - playerDefense);

            if (random.NextDouble() < CRIT_CHANCE)
            {
                Console.WriteLine("Гоблин наносит критический удар!");
                return (int)(baseDamage * CRIT_MULTIPLIER);
            }

            return baseDamage;
        }

        public override string GetSpecialAbility()
        {
            return "Шанс критического удара (20%)";
        }
    }

    public class Skeleton : Enemy
    {
        public Skeleton() : base("Скелет", 35, 7, 4) { }

        public override int CalculateDamage(int playerDefense)
        {
            Console.WriteLine("Скелет игнорирует вашу защиту!");
            return Attack;
        }

        public override string GetSpecialAbility()
        {
            return "Игнорирует защиту игрока";
        }
    }

    public class Mage : Enemy
    {
        private const double FREEZE_CHANCE = 0.25;

        public Mage() : base("Маг", 25, 10, 2) { }

        public override int CalculateDamage(int playerDefense)
        {
            int damage = Math.Max(1, Attack - playerDefense);
            return damage;
        }

        public bool TryFreeze()
        {
            return random.NextDouble() < FREEZE_CHANCE;
        }

        public override string GetSpecialAbility()
        {
            return "Шанс заморозки (25%) - пропуск хода";
        }
    }
    public class VVG : Goblin
    {
        public VVG() : base()
        {
            Name = "ВВГ (Босс Гоблин)";
            MaxHP = (int)(MaxHP * 2.0);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.5);
            Defense = (int)(Defense * 1.2);
        }

        public override int CalculateDamage(int playerDefense)
        {
            int baseDamage = Math.Max(1, Attack - playerDefense);
            double critChance = 0.3;

            if (random.NextDouble() < critChance)
            {
                Console.WriteLine("ВВГ наносит мощный критический удар!");
                return (int)(baseDamage * 1.5);
            }

            return baseDamage;
        }
    }
    public class Kovalsky : Skeleton
    {
        public Kovalsky() : base()
        {
            Name = "Ковальский (Босс Скелет)";
            MaxHP = (int)(MaxHP * 2.5);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.3);
            Defense = (int)(Defense * 1.4);
        }
    }
    public class ArchmageCPP : Mage
    {
        public ArchmageCPP() : base()
        {
            Name = "Архимаг C++ (Босс Маг)";
            MaxHP = (int)(MaxHP * 1.8);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.6);
            Defense = (int)(Defense * 1.1);
        }

        public new bool TryFreeze()
        {
            return random.NextDouble() < 0.35;
        }
    }

    public class PestovC : Skeleton
    {
        private const double FREEZE_CHANCE = 0.4;

        public PestovC() : base()
        {
            Name = "Пестов С-- (Босс Скелет)";
            MaxHP = (int)(MaxHP * 1.3);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.8);
            Defense = (int)(Defense * 0.6);
        }

        public override int CalculateDamage(int playerDefense)
        {
            Console.WriteLine("Пестов С-- игнорирует вашу защиту!");
            return Attack;
        }

        public bool TryFreeze()
        {
            return random.NextDouble() < FREEZE_CHANCE;
        }

        public override string GetSpecialAbility()
        {
            return "Игнорирует защиту + шанс заморозки (40%)";
        }
    }
    public class Player : Creature
    {
        public Weapon CurrentWeapon { get; private set; }
        public Armor CurrentArmor { get; private set; }
        public int TurnCount { get; set; }
        public bool IsFrozen { get; set; }

        private Random random;

        public Player() : base("Игрок", 100, 10, 5)
        {
            CurrentWeapon = new Weapon("Ржавый меч", 5);
            CurrentArmor = new Armor("Кожанная броня", 3);
            random = new Random();
            UpdateStats();
        }

        private void UpdateStats()
        {
            Attack = 10 + CurrentWeapon.Attack;
            Defense = 5 + CurrentArmor.Defense;
        }

        public void EquipWeapon(Weapon newWeapon)
        {
            CurrentWeapon = newWeapon;
            UpdateStats();
        }

        public void EquipArmor(Armor newArmor)
        {
            CurrentArmor = newArmor;
            UpdateStats();
        }

        public int CalculateDamage(int enemyDefense)
        {
            return Math.Max(1, Attack - enemyDefense);
        }

        public bool TryDodge()
        {
            return random.NextDouble() < 0.4;
        }

        public int CalculateBlock(int incomingDamage)
        {
            double blockPercentage = 0.7 + (random.NextDouble() * 0.3);
            int blockedDamage = (int)(Defense * blockPercentage);
            return Math.Max(0, incomingDamage - blockedDamage);
        }

        public void UseHealingPotion()
        {
            Heal(MaxHP);
            Console.WriteLine("Вы использовали лечебное зелье! Здоровье полностью восстановлено.");
        }

        public override void DisplayStats()
        {
            Console.WriteLine("Игрок");
            Console.WriteLine($"HP: {CurrentHP}/{MaxHP}");
            Console.Write($"Оружие: "); CurrentWeapon.DisplayStats();
            Console.Write($"Доспехи: "); CurrentArmor.DisplayStats();
            Console.WriteLine($"Общая атака: {Attack}");
            Console.WriteLine($"Общая защита: {Defense}");
            Console.WriteLine($"Ход: {TurnCount}");
        }
    }
    public class Game
    {
        private Player player;
        private Random random;
        private List<Weapon> availableWeapons;
        private List<Armor> availableArmors;

        public Game()
        {
            player = new Player();
            random = new Random();
            InitializeItems();
        }

        private void InitializeItems()
        {
            availableWeapons = new List<Weapon>
            {
                new Weapon("Деревянный меч", 3),
                new Weapon("Железный меч", 8),
                new Weapon("Стальной меч", 12),
                new Weapon("Мифрильный меч", 16),
                new Weapon("Легендарный меч", 20)
            };

            availableArmors = new List<Armor>
            {
                new Armor("Тряпичная броня", 2),
                new Armor("Кожанная броня", 5),
                new Armor("Кольчуга", 8),
                new Armor("Латная броня", 12),
                new Armor("Драконья броня", 16)
            };
        }