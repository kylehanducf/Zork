using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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

        [JsonIgnore]
        public IEnumerable<Item> Inventory => _inventory;

        [JsonProperty]
        private string[] InventoryNames { get; set; }

        public bool Open { get; set; } 

        public Item(string name, string lookDescription, string inventoryDescription, string tag, string usedMessage, string completedMessage, int damagePoints, bool droppable, bool consumable, string status, string[] inventoryNames, bool open)
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
            InventoryNames = inventoryNames ?? new string[0];
            _inventory = new List<Item>();
            Open = open;
        }

        public void UpdateInventory(World world)
        {
            foreach (var inventoryName in InventoryNames)
            {
                _inventory.Add(world.ItemsByName[inventoryName]);
            }

            InventoryNames = null;
        }

        public void RemoveItemFromInventory(Item itemToRemove)
        {
            if (_inventory.Remove(itemToRemove) == false)
            {
                throw new Exception("Could not remove item from inventory.");
            }
            _inventory.Remove(itemToRemove);
        }

        public override string ToString() => Name;
        private readonly List<Item> _inventory;
    }
}