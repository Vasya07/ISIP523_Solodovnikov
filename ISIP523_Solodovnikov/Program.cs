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
            double critChance = 0.3; // +10% к обычному гоблину

            if (random.NextDouble() < critChance)
            {
                Console.WriteLine("ВВГ наносит мощный критический удар!");
                return (int)(baseDamage * 1.5);
            }

            return baseDamage;
        }
    }