using System;
using System.Linq;
using Newtonsoft.Json;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        [JsonIgnore]
        public Player Player { get; }

        [JsonIgnore]
        public IInputService Input { get; private set; }

        [JsonIgnore]
        public IOutputService Output { get; private set; }

        [JsonIgnore]
        public bool IsRunning { get; private set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }

        public void Run(IInputService input, IOutputService output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));

            IsRunning = true;
            Input.InputReceived += OnInputReceived;
            Output.WriteLine("Welcome to Zork!");
            Look();
            Output.WriteLine($"\n{Player.CurrentRoom}");
        }

        public void OnInputReceived(object sender, string inputString)
        {
            char separator = ' ';
            string[] commandTokens = inputString.Split(separator);

            string verb;
            string subject = null;
            string preposition = null;
            string target = null;

            if (commandTokens.Length == 0)
            {
                return;
            }
            else if (commandTokens.Length == 1)
            {
                verb = commandTokens[0];
            }
            else if (commandTokens.Length == 2)
            {
                verb = commandTokens[0];
                target = commandTokens[1];
            }
            else if (commandTokens.Length == 3)
            {
                return;
            }
            else
            {
                verb = commandTokens[0];
                target = commandTokens[1];
                preposition = commandTokens[2];
                subject = commandTokens[3];
            }

            Room previousRoom = Player.CurrentRoom;
            Commands command = ToCommand(verb);

            if (command != Commands.Score && command != Commands.Unknown)
            {
                Player.AddMove();
            }

            switch (command)
            {
                case Commands.Quit:
                    IsRunning = false;
                    Output.WriteLine("Thank you for playing!");
                    break;

                case Commands.Look:
                    Look();
                    break;

                case Commands.North:
                case Commands.South:
                case Commands.East:
                case Commands.West:
                    Directions direction = (Directions)command;
                    Output.WriteLine(Player.Move(direction) ? $"You moved {direction}." : "The way is shut!");
                    break;

                case Commands.Take:
                    if (string.IsNullOrEmpty(target))
                    {
                        Output.WriteLine("This command requires a target.");
                    }
                    else
                    {
                        Take(target);
                    }
                    break;

                case Commands.Drop:
                    if (string.IsNullOrEmpty(target))
                    {
                        Output.WriteLine("This command requires a target.");
                    }
                    else
                    {
                        Drop(target);
                    }
                    break;

                case Commands.Inventory:
                    if (Player.Inventory.Count() == 1)
                    {
                        Console.WriteLine("You are empty handed.");
                    }
                    else
                    {
                        Console.WriteLine("You are carrying:");
                        foreach (Item item in Player.Inventory)
                        {
                            Output.WriteLine(item.InventoryDescription);
                        }
                    }
                    break;

                case Commands.Attack:
                    if (string.IsNullOrEmpty(target))
                    {
                        Output.WriteLine("This command requires a target.");
                    }
                    if (string.IsNullOrEmpty(preposition))
                    {
                        Output.WriteLine("This command requires a preposition.");
                    }
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Attack(target, subject);
                    }
                    //DONT FORGET TO CLEAN UP THE WAY THIS OUTPUTS TO SCREEN!!!!!!!!!!!!!!!
                    return;

                case Commands.Drink:
                    if (string.IsNullOrEmpty(target))
                    {
                        Output.WriteLine("This command requires a target.");
                    }
                    else
                    {
                        Consume(target);
                    }
                    return;

                case Commands.Reward:
                    Player.AddScore();
                    break;

                case Commands.Score:
                    Output.WriteLine($"Your score would be {Player.Score}, in {Player.Moves} moves.");
                    break;

                default:
                    Output.WriteLine("Unknown command.");
                    break;
            }

            if (ReferenceEquals(previousRoom, Player.CurrentRoom) == false)
            {
                Look();
            }
                        
            Output.WriteLine($"\n{Player.CurrentRoom}");
        }
        
        private void Look()
        {
            Output.WriteLine(Player.CurrentRoom.Description);
            foreach (Item item in Player.CurrentRoom.Inventory)
            {
                Output.WriteLine(item.LookDescription);
            }
            foreach (Enemy enemy in Player.CurrentRoom.Enemies)
            {
                Output.WriteLine(enemy.LookDescription);
            }
        }

        private void Take(string itemName)
        {
            Item itemToTake = Player.CurrentRoom.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToTake == null)
            {
                Console.WriteLine("You can't see any such thing.");                
            }
            else
            {
                Player.AddItemToInventory(itemToTake);
                Player.CurrentRoom.RemoveItemFromInventory(itemToTake);
                Console.WriteLine("Taken.");
            }
        }

        private void Drop(string itemName)
        {
            Item itemToDrop = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToDrop == null || itemToDrop.Droppable == false)
            {
                Console.WriteLine("Huh?");                
            }
            else
            {
                Player.CurrentRoom.AddItemToInventory(itemToDrop);
                Player.RemoveItemFromInventory(itemToDrop);
                Console.WriteLine("Dropped.");
            }
        }

        private void Attack(string enemyName, string weaponName)
        {
            Enemy enemyToAttack = Player.CurrentRoom.Enemies.FirstOrDefault(enemy => string.Compare(enemy.Name, enemyName, ignoreCase: true) == 0);
            Item weaponToAttack = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, weaponName, ignoreCase: true) == 0);
            if (enemyToAttack == null || weaponToAttack == null)
            {
                Console.WriteLine("Do what now?");
            }
            else if (weaponToAttack.Tag != "Weapon")
            {
                Console.WriteLine("That is probably not a good idea.");
            }
            else
            {
                if (enemyToAttack.HitPoints > 0)
                {
                    Player.DealDamage(weaponToAttack, enemyToAttack);
                }
                else
                {
                    Console.WriteLine("It's already dead...");
                }
            }
        }

        private void Consume(string itemName)
        {
            Item itemToConsume = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToConsume == null || itemToConsume.Consumable == false)
            {
                Console.WriteLine("That would be unwise.");
            }
            else
            {
                Player.Consume(itemToConsume);
            }
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
    }
}