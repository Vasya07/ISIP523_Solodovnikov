using TextRoguelike.Entities;
using TextRoguelike.Items;
using TextRoguelike.Factories;
using System.Media;

namespace TextRoguelike
{
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
            Console.WriteLine("1 - Пропустить до 99-го хода (получить +100 HP)");
            Console.WriteLine("2 - Пропустить до 249-го хода (получить Меч Богов)");
            Console.WriteLine("3 - Пропустить до 499-го хода (получить +100 HP)");
            Console.WriteLine("4 - Пропустить до 749-го хода (получить +200 HP)");
            Console.WriteLine("5 - Полное восстановление здоровья");
            Console.WriteLine("6 - Выиграть игру (установить 1000 уровень)");
            Console.WriteLine("7 - Проиграть игру (установить 0 HP)");
            Console.WriteLine("8 - Выйти из режима разработчика");
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
                        player.DeveloperSetTurnCount(249);
                        Console.WriteLine("Установлен 249-й ход");
                        break;

                    case "3":
                        player.DeveloperSetTurnCount(499);
                        Console.WriteLine("Установлен 499-й ход");
                        break;

                    case "4":
                        player.DeveloperSetTurnCount(749);
                        Console.WriteLine("Установлен 749-й ход");
                        break;

                    case "5":
                        player.Heal(player.MaxHP);
                        Console.WriteLine("Здоровье полностью восстановлено!");
                        break;

                    case "6":
                        player.DeveloperSetTurnCount(9999);
                        Console.WriteLine("Установлен 1000-й ход - ПОБЕДА!");
                        break;

                    case "7":
                        player.TakeDamage(player.CurrentHP);
                        Console.WriteLine("Здоровье установлено на 0");
                        break;

                    case "8":
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
            Console.WriteLine("На 100, 250, 500 и 750 уровнях Вас ждут особыем награды");
            Console.WriteLine("Всего в игре 10000 уровней");
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

                if (player.TurnCount >= 10000)
                {
                    WinGame();
                    return;
                }

                player.CheckMilestoneRewards(player.TurnCount);

                if (player.TurnCount % 10 == 0)
                {
                    Console.WriteLine("Внимание, впереди босс!");
                    SoundPlayer boss_coming = new("Sounds/Boss_coming.wav");
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

            if (player.HasGodSword)
            {
                enemy.EnhanceEnemy();
            }

            Console.WriteLine($"Вы встретили {enemy.Name}!");
            if (player.HasGodSword)
            {
                Console.WriteLine("Этот враг усилен силой Меча Богов!");
            }
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
                SoundPlayer win = new("Sounds/Enemy_Win.wav");
                win.Play();
                Console.WriteLine($"Вы победили {enemy.Name}!");
            }
        }

        private void FightBoss()
        {
            SoundPlayer boss_win = new(@"Sounds/Boss_Win.wav");
            Enemy boss = CreateRandomBoss();
            if (player.HasGodSword)
            {
                boss.EnhanceEnemy();
            }

            Console.WriteLine($"\nПоявился босс: {boss.Name}!");
            if (player.HasGodSword)
            {
                Console.WriteLine("Этот босс усилен силой Меча Богов!");
            }
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
                boss_win.Play();
                Console.WriteLine("Нажмите любую клавишу для продолжения");
                Console.ReadKey();
                Console.Clear();
            }
            boss_win.Stop();
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

            if (enemy is Entities.Enemies.Mage mage)
            {
                if (mage.TryFreeze())
                {
                    Console.WriteLine("Маг замораживает вас! Вы пропустите следующий ход");
                    player.IsFrozen = true;
                }
                damageToPlayer = mage.CalculateDamage(player.Defense);
            }
            else if (enemy is Entities.Bosses.PestovC pestov)
            {
                if (pestov.TryFreeze())
                {
                    Console.WriteLine("Пестов С-- замораживает вас! Вы пропустите следующий ход");
                    player.IsFrozen = true;
                }
                damageToPlayer = pestov.CalculateDamage(player.Defense);
            }
            else if (enemy is Entities.Bosses.ArchmageCPP archmage)
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
            Console.WriteLine("Вы нашли сундук!");

            int itemType = random.Next(3);

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

                    Console.Write("\nВзять новое оружие? (y/n): ");

                    string weaponChoice = ReadLineWithDeveloperCheck();

                    if (developerMode)
                    {
                        HandleDeveloperInput();
                        if (!developerMode) Console.Clear();
                        return;
                    }

                    if (weaponChoice.ToLower() == "y")
                    if (weaponChoice.ToLower() == "y")
                    {
                        player.EquipWeapon(newWeapon);
                        Console.WriteLine("\nВы экипировали новое оружие!");
                    }
                    break;

                case 2:
                    Armor newArmor = availableArmors[random.Next(availableArmors.Count)];
                    Console.WriteLine($"\nВ сундуке доспехи: ");
                    newArmor.DisplayStats();
                    Console.WriteLine($"\nВаши текущие доспехи: ");
                    player.CurrentArmor.DisplayStats();

                    Console.Write("\nВзять новые доспехи? (y/n): ");

                    string armorChoice = ReadLineWithDeveloperCheck();

                    if (developerMode)
                    {
                        HandleDeveloperInput();
                        if (!developerMode) Console.Clear();
                        return;
                    }

                    if (armorChoice.ToLower() == "y")
                    {
                        player.EquipArmor(newArmor);
                        Console.WriteLine("\nВы экипировали новые доспехи!");
                    }
                    break;
            }
        }

        private void GameOver()
        {
            if (player.TurnCount < 1000)
            {
                Console.WriteLine("\nИгра окончена");
                Console.WriteLine($"Вы продержались {player.TurnCount} ходов");
                Console.WriteLine("Спасибо за игру!");
                SoundPlayer lose_game = new("Sounds/Lose.wav");
                lose_game.Play();
            }

            Console.WriteLine("Нажмите любую клавишу для выхода");
            Console.ReadKey();
        }
        private void WinGame()
        {
            Console.Clear();
            SoundPlayer winSound = new("Sounds/Game_Win.wav");
            winSound.Play();
            Console.WriteLine("ПОБЕДА!!!");
            Thread.Sleep(2000);
            Console.Write($"Вы достигли");
            Thread.Sleep(500);
            Console.Write($" {player.TurnCount} уровня и прошли игру!\n");
            Thread.Sleep(2000);
            Console.WriteLine("Спасибо за игру!");

            Console.WriteLine("\nНажмите любую клавишу для выхода");
            Console.ReadKey();
        }

        private Enemy CreateRandomEnemy()
        {
            return EnemyFactory.CreateRandomEnemy();
        }

        private Enemy CreateRandomBoss()
        {
            return EnemyFactory.CreateRandomBoss();
        }
    }
}   