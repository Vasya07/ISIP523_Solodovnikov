namespace TextRoguelike.Entities.Enemies
{
    public class Slime : Enemy
    {
        public Slime() : base("Слизень", 40, 6, 1) { }

        public override int CalculateDamage(int playerDefense)
        {
            return Math.Max(1, Attack - playerDefense);
        }

        public override void TakeDamage(int damage)
        {
            int reducedDamage = Math.Max(1, damage - 2);
            base.TakeDamage(reducedDamage);
            Console.WriteLine($"Слизень поглощает часть урона! Получено урона: {reducedDamage}");
        }

        public override string GetSpecialAbility()
        {
            return "Уменьшает входящий урон на 2 единицы";
        }
    }
}