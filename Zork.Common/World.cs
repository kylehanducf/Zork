using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Zork.Common
{
    public class World
    {
        public Room[] Rooms { get; }

        [JsonIgnore]
        public IReadOnlyDictionary<string, Room> RoomsByName => _roomsByName;

        public Item[] Items { get; }

        [JsonIgnore]
        public IReadOnlyDictionary<string, Item> ItemsByName => _itemsByName;

        public Enemy[] Enemies { get; }

        [JsonIgnore]
        public IReadOnlyDictionary<string, Enemy> EnemiesByName => _enemiesByName;

        public World(Room[] rooms, Item[] items, Enemy[] enemies)
        {
            Rooms = rooms;
            _roomsByName = new Dictionary<string, Room>(StringComparer.OrdinalIgnoreCase);
            foreach (Room room in rooms)
            {
                _roomsByName.Add(room.Name, room);
            }

            Items = items;
            _itemsByName = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);
            foreach (Item item in Items)
            {
                _itemsByName.Add(item.Name, item);
            }

            Enemies = enemies;
            _enemiesByName = new Dictionary<string, Enemy>(StringComparer.OrdinalIgnoreCase);
            foreach (Enemy enemy in Enemies)
            {
                _enemiesByName.Add(enemy.Name, enemy);
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext streamingContext)
        {
            foreach (Room room in Rooms)
            {
                room.UpdateNeighbors(this);
                room.UpdateInventory(this);
                room.UpdateEnemies(this);
            }
        }

        private readonly Dictionary<string, Room> _roomsByName;
        private readonly Dictionary<string, Item> _itemsByName;
        private readonly Dictionary<string, Enemy> _enemiesByName;
    }
}