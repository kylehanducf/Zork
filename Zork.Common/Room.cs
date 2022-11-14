using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Zork.Common
{
    public class Room
    {
        public string Name { get; }

        public string Description { get; set; }

        [JsonIgnore]
        public IReadOnlyDictionary<Directions, Room> Neighbors => _neighbors;

        [JsonProperty]
        private Dictionary<Directions, string> NeighborNames { get; set; }

        [JsonIgnore]
        public IEnumerable<Item> Inventory => _inventory;

        [JsonProperty]
        private string[] InventoryNames { get; set; }

        public Room(string name, string description, Dictionary<Directions, string> neighborNames, string[] inventoryNames)
        {
            Name = name;
            Description = description;
            NeighborNames = neighborNames ?? new Dictionary<Directions, string>();
            _neighbors = new Dictionary<Directions, Room>();

            InventoryNames = inventoryNames ?? new string[0];
            _inventory = new List<Item>();
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

        public static bool operator !=(Room lhs, Room rhs) => !(lhs == rhs);

        public override bool Equals(object obj) => obj is Room other && other == this;

        public override int GetHashCode() => Name.GetHashCode();

        public void UpdateNeighbors(World world)
        {            
            foreach (var neighborName in NeighborNames)
            {
                _neighbors.Add(neighborName.Key, world.RoomsByName[neighborName.Value]);
            }

            NeighborNames = null;
        }

        public void UpdateInventory(World world)
        {
            foreach (var inventoryName in InventoryNames)
            {
                _inventory.Add(world.ItemsByName[inventoryName]);
            }

            InventoryNames = null;
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            if (_inventory.Contains(itemToAdd))
            {
                throw new Exception($"Item {itemToAdd} already exists in inventory.");
            }

            _inventory.Add(itemToAdd);
        }

        public void RemoveItemFromInventory(Item itemToRemove)
        {
            if (_inventory.Remove(itemToRemove) == false)
            {
                throw new Exception("Could not remove item from inventory.");
            }
        }

        public override string ToString() => Name;

        private readonly List<Item> _inventory;
        private readonly Dictionary<Directions, Room> _neighbors;
    }
}