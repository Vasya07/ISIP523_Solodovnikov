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