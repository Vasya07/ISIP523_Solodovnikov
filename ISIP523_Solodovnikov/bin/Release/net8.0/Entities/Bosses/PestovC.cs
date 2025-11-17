namespace TextRoguelike.Entities.Bosses
{
    public class PestovC : Entities.Enemies.Skeleton
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
}