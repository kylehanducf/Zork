namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string LookDescription { get; }

        public string InventoryDescription { get; }

        public string Tag { get; }

        public string UsedMessage { get; }

        public string CompletedMessage { get; }

        public int DamagePoints { get; set; }

        public bool Droppable { get; }

        public bool Consumable { get; }

        public string Status { get; }

        public Item(string name, string lookDescription, string inventoryDescription, string tag, string usedMessage, string completedMessage, int damagePoints, bool droppable, bool consumable, string status)
        {
            Name = name;
            LookDescription = lookDescription;
            InventoryDescription = inventoryDescription;
            Tag = tag;
            UsedMessage = usedMessage;
            CompletedMessage = completedMessage;
            DamagePoints = damagePoints;
            Droppable = droppable;
            Consumable = consumable;
            Status = status;
        }

        public override string ToString() => Name;
    }
}