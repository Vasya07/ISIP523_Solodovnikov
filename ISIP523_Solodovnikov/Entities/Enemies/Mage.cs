namespace TextRoguelike.Entities.Enemies
{
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
}