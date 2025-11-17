namespace TextRoguelike.Entities.Enemies
{
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
}