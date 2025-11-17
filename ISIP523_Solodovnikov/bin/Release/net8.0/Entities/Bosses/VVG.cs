namespace TextRoguelike.Entities.Bosses
{
    public class VVG : Entities.Enemies.Goblin
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
                System.Media.SoundPlayer critcial_hint_vvg = new("Sounds/Critical_Hint.wav");
                critcial_hint_vvg.Play();
                return Math.Max(1, (int)(baseDamage * 1.5));
            }

            return finalDamage;
        }
    }
}