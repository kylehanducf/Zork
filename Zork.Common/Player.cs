using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;

namespace Zork.Common
{
    public class Player
    {
        public Room CurrentRoom
        {
            get => _currentRoom;
            set => _currentRoom = value;
        }

        public List<Item> Inventory { get; }


        public Player(World world, string startingLocation)
        {
            _world = world;

            if (_world.RoomsByName.TryGetValue(startingLocation, out _currentRoom) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}");
            }

            Inventory = new List<Item>();
        }

        public bool Move(Directions direction)
        {
            bool didMove = _currentRoom.Neighbors.TryGetValue(direction, out Room neighbor);
            if (didMove)
            {
                CurrentRoom = neighbor;
            }

            return didMove;
        }

        public void Take(string itemName)
        {
            Item itemToTake = null;
            foreach (Item item in CurrentRoom.Inventory)
            {
                if (string.Compare(itemName, item.Name, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    itemToTake = item;
                    AddToInventory(itemToTake);
                    CurrentRoom.RemoveFromInventory(itemToTake);
                    Console.WriteLine("Taken");
                    break;
                }
            }

            if (itemToTake == null)
            {
                Console.WriteLine("No such item exists.");
            }

        }

        public void Drop(string itemName)
        {
            Item itemToDrop = null;
            foreach (Item item in Inventory)
            {
                if (string.Compare(itemName, item.Name, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    itemToDrop = item;
                    RemoveFromInventory(itemToDrop);
                    CurrentRoom.AddToInventory(itemToDrop);
                    Console.WriteLine("Dropped");
                    break;
                }
            }

            if (itemToDrop == null)
            {
                Console.WriteLine("You don't have that.");
            }

        }

        public void AddToInventory(Item itemToAdd)
        {
            Inventory.Add(itemToAdd);
        }

        public void RemoveFromInventory(Item itemToRemove)
        {
            Inventory.Remove(itemToRemove);
        }

        private World _world;
        private Room _currentRoom;
    }
}
