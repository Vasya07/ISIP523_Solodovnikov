namespace TextRoguelike.Entities.Bosses
{
    public class ArchmageCPP : Entities.Enemies.Mage
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
}