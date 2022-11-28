namespace Zork.Common
{
    public class Enemy
    {
        public string Name { get; }

        public string LookDescription { get; }

        public int HitPoints { get; }

        public Enemy(string name, string lookDescription, string enemyLocation, int hitPoints)
        {
            Name = name;
            LookDescription = lookDescription;
            HitPoints = hitPoints;
        }

        public override string ToString() => Name;

    }
}
