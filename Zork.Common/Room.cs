using Newtonsoft.Json;
using System.Collections.Generic;

namespace Zork.Common
{
    public class Room
    {
        public string Name { get; }

        public string Description { get; set; }

        [JsonIgnore]
        public Dictionary<Directions, Room> Neighbors { get; private set; }

        [JsonProperty]
        private Dictionary<Directions, string> NeighborNames { get; set; }

        [JsonIgnore]
        public List<Item> Inventory { get; private set; }

        [JsonProperty]
        private string[] InventoryNames { get; set; }

        public Room(string name, string description, Dictionary<Directions, string> neighborNames, string[] inventoryNames)
        {
            Name = name;
            Description = description;
            NeighborNames = neighborNames ?? new Dictionary<Directions, string>();
            InventoryNames = inventoryNames ?? new string[0];
        }

        public static bool operator ==(Room lhs, Room rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if (lhs is null || rhs is null)
            {
                return false;
            }

            return string.Compare(lhs.Name, rhs.Name, ignoreCase: true) == 0;
        }

        public static bool operator !=(Room lhs, Room rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return obj is Room other && other == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void UpdateNeighbors(World world)
        {
            Neighbors = new Dictionary<Directions, Room>();
            foreach (var neighborName in NeighborNames)
            {
                Neighbors.Add(neighborName.Key, world.RoomsByName[neighborName.Value]);
            }

            NeighborNames = null;
        }

        public void UpdateInventory(World world)
        {
            Inventory = new List<Item>();
            foreach (var inventoryName in InventoryNames)
            {
                Inventory.Add(world.ItemsByName[inventoryName]);
            }

            InventoryNames = null;
        }

        public override string ToString() => Name;
    }
}