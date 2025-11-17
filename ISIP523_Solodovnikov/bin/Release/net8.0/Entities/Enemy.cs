namespace TextRoguelike.Entities
{
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
}