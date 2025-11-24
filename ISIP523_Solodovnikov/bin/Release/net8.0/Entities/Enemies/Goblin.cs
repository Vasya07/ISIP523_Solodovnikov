namespace TextRoguelike.Entities.Enemies
{
    public class Goblin : Enemy
    {
        private const double CRIT_CHANCE = 0.5;
        private const double CRIT_MULTIPLIER = 1.5;

        public Goblin() : base("Гоблин", 30, 8, 3) { }

        public override int CalculateDamage(int playerDefense)
        {
            int baseDamage = Attack;
            int finalDamage = Math.Max(1, baseDamage - playerDefense);

            if (random.NextDouble() < CRIT_CHANCE)
            {
                Console.WriteLine("Гоблин наносит критический удар!");
                System.Media.SoundPlayer critcial_hint = new("Sounds/Critical_Hint.wav");
                critcial_hint.Play();
                return Math.Max(1, (int)(baseDamage * CRIT_MULTIPLIER));
            }

            return finalDamage;
        }

        public override string GetSpecialAbility()
        {
            return "Шанс критического удара - 50%";
        }
    }
}