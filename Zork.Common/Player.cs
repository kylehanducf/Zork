using System;
using System.Collections.Generic;
using System.Linq;

namespace Zork.Common
{
    public class Player
    {
        public event EventHandler<Room> LocationChanged;

        public event EventHandler<int> ScoreChanged;

        public event EventHandler<int> MovesChanged;

        public Room CurrentRoom
        {
            get => _currentRoom;
            set
            {
                if (_currentRoom != value)
                {
                    _currentRoom = value;
                    LocationChanged?.Invoke(this, _currentRoom);
                }
            } 
        }

        public IEnumerable<Item> Inventory => _inventory;

        public int Score { get; set; }

        public int Moves { get; set; }

        public Player(World world, string startingLocation)
        {
            _world = world;

            if (_world.RoomsByName.TryGetValue(startingLocation, out _currentRoom) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}");
            }

            _inventory = new List<Item>();
            foreach (Item item in world.Items)
            {
                if (item.Name == "Fists")
                {
                    _inventory.Add(item);
                }
            }
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

        public void AddMove()
        {
            Moves++;
            MovesChanged?.Invoke(this, Moves);
        }

        public void AddScore()
        {
            Score++;
            ScoreChanged?.Invoke(this, Score);
        }

        public void DealDamage(Item attackingItem, Enemy attackedEnemy)
        {
            attackedEnemy.HitPoints -= attackingItem.DamagePoints;
            if(attackedEnemy.HitPoints <= 0)
            {
                Console.WriteLine(attackedEnemy.DeathDescription);
                attackedEnemy.LookDescription = attackedEnemy.DeathDescription;
                Console.WriteLine($"{attackingItem.CompletedMessage} \n");
            }
            else
            {
                Console.WriteLine($"{attackingItem.UsedMessage} \n");
            }
        }

        public void Consume(Item consumableItem)
        {
            switch (consumableItem.Status)
            {
                case "Buff Strength":
                    foreach (Item item in _inventory)
                    {
                        if(item.Name == "Fists")
                        {
                            item.DamagePoints += 10;
                            Console.WriteLine(consumableItem.CompletedMessage);
                        }
                    }
                    return;
            }
        }
        public void Open(Item itemToOpen)
        {
            if(itemToOpen.Open == false)
            {
                Console.WriteLine("You empty its contents.");

                foreach (Item item in itemToOpen.Inventory)
                {
                    _currentRoom.AddItemToInventory(item);
                    Console.WriteLine(item.LookDescription);
                }

                Console.WriteLine("");
                itemToOpen.Open = true;
            }
            else
            {
                Console.WriteLine("It's already open.\n");
            }
        }

        private readonly World _world;
        private Room _currentRoom;
        private readonly List<Item> _inventory;
    }
}
