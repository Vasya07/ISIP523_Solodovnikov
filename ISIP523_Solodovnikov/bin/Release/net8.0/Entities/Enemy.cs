namespace TextRoguelike.Entities
{
    public abstract class Enemy : Creature
    {
        protected Random random;
        protected bool isEnhanced;

        public Enemy(string name, int maxHP, int attack, int defense) : base(name, maxHP, attack, defense)
        {
            random = new Random();
            isEnhanced = false;
        }

        public void EnhanceEnemy()
        {
            if (!isEnhanced)
            {
                MaxHP *= 2;
                CurrentHP = MaxHP;
                Attack *= 2;
                isEnhanced = true;
            }
        }

        public abstract int CalculateDamage(int playerDefense);
        public abstract string GetSpecialAbility();

        public override void DisplayStats()
        {
            string enhancedIndicator = isEnhanced ? "" : "";
            Console.WriteLine($"{enhancedIndicator}{Name} - HP: {CurrentHP}/{MaxHP}, Атака: {Attack}, Защита: {Defense}");
        }
    }
}