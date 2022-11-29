namespace Zork.Common
{
    public class Enemy
    {
        public string Name { get; }

        public string LookDescription { get; set; }

        public string DeathDescription { get; }

        public int HitPoints { get; set; }

        public Enemy(string name, string lookDescription, string enemyLocation, int hitPoints, string deathDescription)
        {
            Name = name;
            LookDescription = lookDescription;
            HitPoints = hitPoints;
            DeathDescription = deathDescription;
        }

        public override string ToString() => Name;

    }
}
