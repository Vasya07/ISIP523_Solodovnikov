using TextRoguelike.Items;

namespace TextRoguelike.Entities
{
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

        public void CheckMilestoneRewards(int turnCount)
        {
            switch (turnCount)
            {
                case 100:
                    IncreaseMaxHP(100, "200_HP.wav");
                    break;
                case 250:
                    GiveGodSword();
                    break;
                case 500:
                    IncreaseMaxHP(100, "300_HP.wav");
                    break;
                case 750:
                    IncreaseMaxHP(200, "Max_HP.wav");
                    break;
            }
        }

        private void IncreaseMaxHP(int amount, string soundFile)
        {
            int oldMaxHP = MaxHP;
            MaxHP += amount;
            CurrentHP = MaxHP;

            Console.WriteLine($"Уровень {TurnCount}");
            Console.WriteLine($"Максимальное HP повышено: {oldMaxHP} -> {MaxHP}");
            Console.WriteLine("Здоровье полностью восстановлено!");

            System.Media.SoundPlayer achievement = new($"Sounds/{soundFile}");
            achievement.Play();
        }

        private void GiveGodSword()
        {
            Weapon godSword = new Weapon("МЕЧ БОГОВ", 50);
            EquipWeapon(godSword);

            Console.WriteLine("Уровень 250");
            Console.WriteLine("Получен: МЕЧ БОГОВ");
            Console.WriteLine("Ваша атака значительно увеличена!");

            System.Media.SoundPlayer achievement = new("Sounds/Secret_Sword.wav");
            achievement.Play();
        }

        public void DeveloperSetTurnCount(int turns)
        {
            TurnCount = turns;
        }

        public int CalculateDamage(int enemyDefense)
        {
            return Math.Max(1, Attack - enemyDefense);
        }

        public bool TryDodge()
        {
            double dodgeChance = 0.4;
            return random.NextDouble() < dodgeChance;
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
            Console.WriteLine("\nВы использовали лечебное зелье! Здоровье полностью восстановлено");
        }

        public override void DisplayStats()
        {
            Console.WriteLine("Игрок");
            Console.WriteLine($"HP: {CurrentHP}/{MaxHP}");
            Console.Write($"Оружие: ");
            CurrentWeapon.DisplayStats();
            Console.Write($"Доспехи: ");
            CurrentArmor.DisplayStats();
            Console.WriteLine($"Общая атака: {Attack}");
            Console.WriteLine($"Общая защита: {Defense}");
            Console.WriteLine($"Ход: {TurnCount}");

            if (MaxHP >= 200)
            {
                Console.Write("Достижения: ");
                if (MaxHP >= 200) Console.Write("Уровень 100 ");
                if (MaxHP >= 300) Console.Write("Уровень 500 ");
                if (MaxHP >= 400) Console.Write("Уровень 750");
                Console.WriteLine("");
            }
        }
    }
}