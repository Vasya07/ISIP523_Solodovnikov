using TextRoguelike.Entities;
using TextRoguelike.Entities.Enemies;
using TextRoguelike.Entities.Bosses;

namespace TextRoguelike.Factories
{
    public static class EnemyFactory
    {
        private static Random random = new Random();

        public static Enemy CreateRandomEnemy()
        {
            int enemyType = random.Next(4);

            return enemyType switch
            {
                0 => new Goblin(),
                1 => new Skeleton(),
                2 => new Mage(),
                3 => new Slime(),
                _ => new Goblin()
            };
        }

        public static Enemy CreateRandomBoss()
        {
            int bossType = random.Next(4);

            return bossType switch
            {
                0 => new VVG(),
                1 => new Kovalsky(),
                2 => new ArchmageCPP(),
                3 => new PestovC(),
                _ => new VVG()
            };
        }

        public static Enemy CreateEnemy(string enemyType)
        {
            return enemyType.ToLower() switch
            {
                "goblin" => new Goblin(),
                "skeleton" => new Skeleton(),
                "mage" => new Mage(),
                "slime" => new Slime(),
                "boss_vvg" => new VVG(),
                "boss_kovalsky" => new Kovalsky(),
                "boss_archmage" => new ArchmageCPP(),
                "boss_pestov" => new PestovC(),
                _ => new Goblin()
            };
        }
    }
}