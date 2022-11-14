namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string LookDescription { get; }

        public string InventoryDescription { get; }

        public Item(string name, string lookDescription, string inventoryDescription)
        {
            Name = name;
            LookDescription = lookDescription;
            InventoryDescription = inventoryDescription;
        }

        public override string ToString() => Name;
    }
}