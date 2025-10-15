using System.Media;

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
            int baseDamage = Attack;
            int finalDamage = Math.Max(1, baseDamage - playerDefense);

            if (random.NextDouble() < CRIT_CHANCE)
            {

                Console.WriteLine("Гоблин наносит критический удар!");
                SoundPlayer critcial_hint = new("Critical_Hint.wav");
                critcial_hint.Play();
                return Math.Max(1, (int)(baseDamage * CRIT_MULTIPLIER) - playerDefense);
            }

            return finalDamage;
        }

        public override string GetSpecialAbility()
        {
            return "Шанс критического удара - 20%";
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
            return "Шанс заморозки - 25% и пропуск хода";
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
            int baseDamage = Attack;
            int finalDamage = Math.Max(1, baseDamage - playerDefense);
            double critChance = 0.3;

            if (random.NextDouble() < critChance)
            {
                Console.WriteLine("ВВГ наносит мощный критический удар!");
                SoundPlayer critcial_hint_vvg = new("Critical_Hint.wav");
                critcial_hint_vvg.Play();
                return Math.Max(1, (int)(baseDamage * 1.5) - playerDefense);
            }

            return finalDamage;
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
            return "Игнорирует защиту + шанс заморозки - 40%";
        }
    }
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
            if (HasSecretSword) Console.Write("✨ ");
            CurrentWeapon.DisplayStats();
            Console.Write($"Доспехи: ");
            if (HasSecretArmor) Console.Write("✨ ");
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
    public class Game
    {
        private Player player;
        private Random random;
        private List<Weapon> availableWeapons;
        private List<Armor> availableArmors;
        private bool developerMode;

        public Game()
        {
            player = new Player();
            random = new Random();
            developerMode = false;
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

        private void ShowDeveloperMenu()
        {
            Console.WriteLine("\nРежим разработчика");
            Console.WriteLine("1 - Пропустить до 99-го хода (получить Меч Богов)");
            Console.WriteLine("2 - Пропустить до 199-го хода (получить Броню Богов)");
            Console.WriteLine("3 - Установить произвольный уровень");
            Console.WriteLine("4 - Получить все секретные предметы");
            Console.WriteLine("5 - Полное восстановление здоровья");
            Console.WriteLine("6 - Выйти из режима разработчика");
            Console.Write("Выберите действие: ");
        }

        private void HandleDeveloperInput()
        {
            while (true)
            {
                Console.Clear();
                player.DisplayStats();
                ShowDeveloperMenu();

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        player.DeveloperSetTurnCount(99);
                        Console.WriteLine("Установлен 99-й ход");
                        break;

                    case "2":
                        player.DeveloperSetTurnCount(199);
                        Console.WriteLine("Установлен 199-й ход");
                        break;

                    case "3":
                        Console.Write("Введите номер хода: ");
                        if (int.TryParse(Console.ReadLine(), out int turns))
                        {
                            player.DeveloperSetTurnCount(turns - 1);
                            Console.WriteLine($"Установлен {turns}-й ход!");

                            if (turns >= 100 && !player.HasSecretSword)
                            {
                                player.DeveloperGiveSecretSword();
                                Console.WriteLine("Автоматически выдан Меч Богов!");
                            }
                            if (turns >= 200 && !player.HasSecretArmor)
                            {
                                player.DeveloperGiveSecretArmor();
                                Console.WriteLine("Автоматически выдана Броня Богов!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат числа!");
                        }
                        break;

                    case "4":
                        player.DeveloperGiveSecretSword();
                        player.DeveloperGiveSecretArmor();
                        Console.WriteLine("Выданы все секретные предметы!");
                        break;

                    case "5":
                        player.Heal(player.MaxHP);
                        Console.WriteLine("Здоровье полностью восстановлено!");
                        break;

                    case "6":
                        developerMode = false;
                        Console.WriteLine("Режим разработчика отключен!");
                        return;

                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения");
                Console.ReadKey();
            }
        }

        private string ReadLineWithDeveloperCheck()
        {
            string input = Console.ReadLine();
            if (input?.ToLower() == "d")
            {
                developerMode = true;
                return input;
            }
            return input;
        }

        public void StartGame()
        {
            Console.WriteLine("Добро пожаловать в игру! Ваша цель - выживать как можно дольше.");
            Console.WriteLine("Каждый ход вы можете встретить врага или найти сундук.");
            Console.WriteLine("Каждые 10 ходов вас ждёт встреча с боссом!");
            Console.WriteLine("НА 100 и 200 уровнях Вас ждут награды. Удачи добраться до них!");
            Console.WriteLine("\nНажмите любую клавишу для старта игры");
            Console.ReadKey();
            Console.Clear();

            while (player.IsAlive)
            {
                player.TurnCount++;
                player.IsFrozen = false;

                Console.WriteLine($"\nХод {player.TurnCount}");
                player.DisplayStats();
                Console.WriteLine();

                if (player.TurnCount == 100 && !player.HasSecretSword)
                {
                    SoundPlayer achievement = new("Secret_Sword.wav");
                    achievement.Play();
                    player.GiveSecretSword();
                    Console.WriteLine("\nНажмите любую клавишу для продолжения");
                    Console.ReadKey();
                }

                if (player.TurnCount == 200 && !player.HasSecretArmor)
                {
                    SoundPlayer achievement = new("Secret_Armor.wav");
                    achievement.Play();
                    player.GiveSecretArmor();
                    Console.WriteLine("\nНажмите любую клавишу для продолжения");
                    Console.ReadKey();
                }

                if (player.TurnCount % 10 == 0)
                {
                    Console.WriteLine("Внимание, впереди босс!");
                    SoundPlayer boss_coming = new("Boss_coming.wav");
                    boss_coming.Play();
                    FightBoss();
                }
                else
                {
                    if (random.Next(2) == 0)
                    {
                        FightEnemy();
                    }
                    else
                    {
                        OpenChest();
                    }
                }

                if (player.IsAlive)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            GameOver();
        }

        private void FightEnemy()
        {
            Enemy enemy = CreateRandomEnemy();
            Console.WriteLine($"Вы встретили {enemy.Name}!");
            Console.WriteLine($"Особенность: {enemy.GetSpecialAbility()}");

            while (enemy.IsAlive && player.IsAlive)
            {
                if (player.IsFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    player.IsFrozen = false;
                }
                else
                {
                    PlayerTurn(enemy);
                    if (!enemy.IsAlive) break;
                }

                if (developerMode)
                {
                    HandleDeveloperInput();
                    if (!developerMode) Console.Clear();
                    return;
                }

                EnemyTurn(enemy);

                if (developerMode)
                {
                    HandleDeveloperInput();
                    if (!developerMode) Console.Clear();
                    return;
                }
            }

            if (player.IsAlive && !developerMode)
            {
                SoundPlayer win = new("Enemy_Win.wav");
                win.Play();
                Console.WriteLine($"Вы победили {enemy.Name}!");
            }
        }

        private void FightBoss()
        {
            Enemy boss = CreateRandomBoss();
            Console.WriteLine($"\nПоявился босс: {boss.Name}!");
            boss.DisplayStats();
            Console.WriteLine();

            while (boss.IsAlive && player.IsAlive)
            {
                if (player.IsFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    player.IsFrozen = false;
                }
                else
                {
                    PlayerTurn(boss);
                    if (!boss.IsAlive) break;
                }

                if (developerMode)
                {
                    HandleDeveloperInput();
                    if (!developerMode) Console.Clear();
                    return;
                }

                EnemyTurn(boss);

                if (developerMode)
                {
                    HandleDeveloperInput();
                    if (!developerMode) Console.Clear();
                    return;
                }
            }

            if (player.IsAlive && !developerMode)
            {
                Console.WriteLine($"Невероятно! Вы победили {boss.Name}!");
                SoundPlayer boss_win = new(@"Boss_Win.wav");
                boss_win.Play();
            }
        }

        private void PlayerTurn(Enemy enemy)
        {
            Console.WriteLine("\nВаш ход");
            Console.WriteLine("1 - Атаковать");
            Console.WriteLine("2 - Защищаться");
            Console.Write("Выберите действие: ");

            string choice = ReadLineWithDeveloperCheck();

            if (developerMode)
            {
                HandleDeveloperInput();
                if (!developerMode) Console.Clear();
                return;
            }

            switch (choice)
            {
                case "1":
                    int damage = player.CalculateDamage(enemy.Defense);
                    enemy.TakeDamage(damage);
                    Console.WriteLine($"Вы нанесли {damage} урона {enemy.Name}!");
                    break;

                case "2":
                    Console.WriteLine("Вы готовитесь к защите");
                    break;

                default:
                    Console.WriteLine("Неверный выбор, вы пропускаете ход!");
                    break;
            }
        }

        private void EnemyTurn(Enemy enemy)
        {
            if (!enemy.IsAlive) return;

            Console.WriteLine($"\nХод {enemy.Name}");

            bool isDefending = false;
            int damageToPlayer = 0;

            if (enemy is Mage mage)
            {
                if (mage.TryFreeze())
                {
                    Console.WriteLine("Маг замораживает вас! Вы пропустите следующий ход");
                    player.IsFrozen = true;
                }
                damageToPlayer = mage.CalculateDamage(player.Defense);
            }
            else if (enemy is PestovC pestov)
            {
                if (pestov.TryFreeze())
                {
                    Console.WriteLine("Пестов С-- замораживает вас! Вы пропустите следующий ход");
                    player.IsFrozen = true;
                }
                damageToPlayer = pestov.CalculateDamage(player.Defense);
            }
            else if (enemy is ArchmageCPP archmage)
            {
                if (archmage.TryFreeze())
                {
                    Console.WriteLine("Архимаг C++ замораживает вас! Вы пропустите следующий ход");
                    player.IsFrozen = true;
                }
                damageToPlayer = archmage.CalculateDamage(player.Defense);
            }
            else
            {
                damageToPlayer = enemy.CalculateDamage(player.Defense);
            }

            if (isDefending && player.TryDodge())
            {
                Console.WriteLine("Вы успешно уклонились от атаки!");
            }
            else if (isDefending)
            {
                int blockedDamage = player.CalculateBlock(damageToPlayer);
                damageToPlayer = blockedDamage;
                Console.WriteLine($"Вы блокируете часть урона! Получено урона: {damageToPlayer}");
            }

            if (damageToPlayer > 0)
            {
                player.TakeDamage(damageToPlayer);
                Console.WriteLine($"{enemy.Name} наносит вам {damageToPlayer} урона!");
            }
            else
            {
                Console.WriteLine($"{enemy.Name} не смог нанести урон!");
            }
        }

        private void OpenChest()
        {
            SoundPlayer chest = new("Chest.wav");
            Console.WriteLine("Вы нашли сундук!");

            int itemType = random.Next(3);
            chest.PlayLooping();

            switch (itemType)
            {
                case 0:
                    Console.WriteLine("В сундуке лечебное зелье!");
                    player.UseHealingPotion();
                    break;

                case 1:
                    Weapon newWeapon = availableWeapons[random.Next(availableWeapons.Count)];
                    Console.WriteLine($"\nВ сундуке оружие: ");
                    newWeapon.DisplayStats();
                    Console.WriteLine($"\nВаше текущее оружие: ");
                    player.CurrentWeapon.DisplayStats();

                    if (player.HasSecretSword)
                    {
                        Console.WriteLine("\nВнимание! У вас есть МЕЧ БОГОВ!");
                        Console.Write("Вы уверены, что хотите расстаться с секретным оружием? (y/n): ");

                        string confirmation = ReadLineWithDeveloperCheck();

                        if (developerMode)
                        {
                            HandleDeveloperInput();
                            chest.Stop();
                            if (!developerMode) Console.Clear();
                            return;
                        }

                        if (confirmation.ToLower() != "y")
                        {
                            Console.WriteLine("Вы оставили новое оружие в сундуке");
                            break;
                        }
                    }

                    Console.Write("\nВзять новое оружие? (y/n): ");

                    string weaponChoice = ReadLineWithDeveloperCheck();

                    if (developerMode)
                    {
                        HandleDeveloperInput();
                        chest.Stop();
                        if (!developerMode) Console.Clear();
                        return;
                    }

                    if (weaponChoice.ToLower() == "y")
                    {
                        player.EquipWeapon(newWeapon);
                        Console.WriteLine("\nВы экипировали новое оружие!");
                        if (player.HasSecretSword && newWeapon.Attack < 50)
                        {
                            player.LoseSecretSword();
                            Console.WriteLine("Вы потеряли МЕЧ БОГОВ!");
                        }
                    }
                    break;

                case 2:
                    Armor newArmor = availableArmors[random.Next(availableArmors.Count)];
                    Console.WriteLine($"\nВ сундуке доспехи: ");
                    newArmor.DisplayStats();
                    Console.WriteLine($"\nВаши текущие доспехи: ");
                    player.CurrentArmor.DisplayStats();

                    if (player.HasSecretArmor)
                    {
                        Console.WriteLine("\nВнимание! У вас есть БРОНЯ БОГОВ!");
                        Console.Write("Вы уверены, что хотите расстаться с секретной бронёй? (y/n): ");

                        string confirmation = ReadLineWithDeveloperCheck();

                        if (developerMode)
                        {
                            HandleDeveloperInput();
                            chest.Stop();
                            if (!developerMode) Console.Clear();
                            return;
                        }

                        if (confirmation.ToLower() != "y")
                        {
                            Console.WriteLine("Вы оставили новые доспехи в сундуке");
                            break;
                        }
                    }

                    Console.Write("\nВзять новые доспехи? (y/n): ");

                    string armorChoice = ReadLineWithDeveloperCheck();

                    if (developerMode)
                    {
                        HandleDeveloperInput();
                        chest.Stop();
                        if (!developerMode) Console.Clear();
                        return;
                    }

                    if (armorChoice.ToLower() == "y")
                    {
                        player.EquipArmor(newArmor);
                        Console.WriteLine("\nВы экипировали новые доспехи!");
                        if (player.HasSecretArmor && newArmor.Defense < 40)
                        {
                            player.LoseSecretArmor();
                            Console.WriteLine("Вы потеряли БРОНЮ БОГОВ!");
                        }
                    }
                    break;
            }
            chest.Stop();
        }

        private void GameOver()
        {
            Console.WriteLine("\nИгра окончена");
            Console.WriteLine($"Вы продержались {player.TurnCount} ходов");

            if (player.HasSecretSword && player.HasSecretArmor)
            {
                Console.WriteLine("Вы получили И МЕЧ БОГОВ И БРОНЮ БОГОВ!");
            }
            else if (player.HasSecretSword)
            {
                Console.WriteLine("Вы получили МЕЧ БОГОВ!");
            }
            else if (player.HasSecretArmor)
            {
                Console.WriteLine("Вы получили БРОНЮ БОГОВ!");
            }
            else if (player.TurnCount >= 190)
            {
                Console.WriteLine("Отличный результат! Вы почти достигли легендарной брони!");
            }
            else if (player.TurnCount >= 90)
            {
                Console.WriteLine("Хороший результат! Вы почти достигли легендарного меча!");
            }

            Console.WriteLine("Спасибо за игру!");
        }

        private Enemy CreateRandomEnemy()
        {
            int enemyType = random.Next(3);

            return enemyType switch
            {
                0 => new Goblin(),
                1 => new Skeleton(),
                2 => new Mage(),
                _ => new Goblin()
            };
        }

        private Enemy CreateRandomBoss()
        {
            int bossType = random.Next(4);

            return bossType switch
            {
                0 => new VVG(),
                1 => new Kovalsky(),
                2 => new ArchmageCPP(),
                3 => new PestovC(),
                _ => new VVG()
            };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.StartGame();
        }
    }
}