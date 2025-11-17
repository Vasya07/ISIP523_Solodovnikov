using TextRoguelike.Items;

namespace TextRoguelike.Entities
{
    public class Player : Creature
    {
        public Weapon CurrentWeapon { get; private set; }
        public Armor CurrentArmor { get; private set; }
        public int TurnCount { get; set; }
        public bool IsFrozen { get; set; }
        public bool HasSecretSword { get; private set; }
        public bool HasSecretArmor { get; private set; }

        private Random random;

        public Player() : base("Игрок", 100, 10, 5)
        {
            CurrentWeapon = new Weapon("Ржавый меч", 5);
            CurrentArmor = new Armor("Кожанная броня", 3);
            random = new Random();
            HasSecretSword = false;
            HasSecretArmor = false;
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

        public void GiveSecretSword()
        {
            if (!HasSecretSword)
            {
                Weapon secretSword = new Weapon("МЕЧ БОГОВ", 50);
                EquipWeapon(secretSword);
                HasSecretSword = true;

                Console.WriteLine("\nПОЗДРАВЛЯЕМ!!!");
                Console.WriteLine("Вы достигли 100-го хода и получили СЕКРЕТНЫЙ МЕЧ БОГОВ!");
                Console.WriteLine("Теперь вы обладаете невероятной силой!");

                Heal(MaxHP);
            }
        }

        public void GiveSecretArmor()
        {
            if (!HasSecretArmor)
            {
                Armor secretArmor = new Armor("БРОНЯ БОГОВ", 40);
                EquipArmor(secretArmor);
                HasSecretArmor = true;

                Console.WriteLine("\nПОЗДРАВЛЯЕМ!!!");
                Console.WriteLine("Вы достигли 200-го хода и получили СЕКРЕТНУЮ БРОНЮ БОГОВ!");
                Console.WriteLine("Теперь вы практически неуязвимы!");

                Heal(MaxHP);
            }
        }

        public void LoseSecretSword()
        {
            HasSecretSword = false;
        }

        public void LoseSecretArmor()
        {
            HasSecretArmor = false;
        }

        public void DeveloperGiveSecretSword()
        {
            GiveSecretSword();
        }

        public void DeveloperGiveSecretArmor()
        {
            GiveSecretArmor();
        }

        public void DeveloperSetTurnCount(int turns)
        {
            TurnCount = turns;
        }

        public int CalculateDamage(int enemyDefense)
        {
            int baseDamage = Math.Max(1, Attack - enemyDefense);

            if (HasSecretSword && random.NextDouble() < 0.3)
            {
                Console.WriteLine("Меч Богов излучает божественную энергию! Урон удвоен!");
                return baseDamage * 2;
            }

            return baseDamage;
        }

        public bool TryDodge()
        {
            double dodgeChance = HasSecretArmor ? 0.6 : 0.4;
            return random.NextDouble() < dodgeChance;
        }

        public int CalculateBlock(int incomingDamage)
        {
            double blockPercentage = HasSecretArmor ?
                0.8 + (random.NextDouble() * 0.4) :
                0.7 + (random.NextDouble() * 0.3);

            int blockedDamage = (int)(Defense * blockPercentage);
            return Math.Max(0, incomingDamage - blockedDamage);
        }

        public void UseHealingPotion()
        {
            Heal(MaxHP);
            Console.WriteLine("\nВы использовали лечебное зелье! Здоровье полностью восстановлено");
        }

        public override void DisplayStats()
        {
            Console.WriteLine("Игрок");
            Console.WriteLine($"HP: {CurrentHP}/{MaxHP}");
            Console.Write($"Оружие: ");
            if (HasSecretSword) Console.Write("");
            CurrentWeapon.DisplayStats();
            Console.Write($"Доспехи: ");
            if (HasSecretArmor) Console.Write("");
            CurrentArmor.DisplayStats();
            Console.WriteLine($"Общая атака: {Attack}");
            Console.WriteLine($"Общая защита: {Defense}");
            Console.WriteLine($"Ход: {TurnCount}");

            if (HasSecretSword || HasSecretArmor)
            {
                Console.Write("Обладатель: ");
                if (HasSecretSword) Console.Write("МЕЧА БОГОВ ");
                if (HasSecretArmor) Console.Write("БРОНИ БОГОВ");
                Console.WriteLine("");
            }
        }
    }
}