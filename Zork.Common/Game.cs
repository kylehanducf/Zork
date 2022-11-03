using System;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        public Player Player { get; }

        public IOutputService Output { get; private set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }

        public void Run(IOutputService output)
        {
            Output = output;

            Room previousRoom = null;
            bool isRunning = true;
            while (isRunning)
            {
                Output.WriteLine(Player.CurrentRoom);
                if (previousRoom != Player.CurrentRoom)
                {
                    Output.WriteLine(Player.CurrentRoom.Description);
                    foreach (Item item in Player.CurrentRoom.Inventory)
                    {
                        Output.WriteLine(item.Description);
                    }
                    previousRoom = Player.CurrentRoom;
                }

                Output.Write("> ");

                string inputString = Console.ReadLine().Trim();
                char  separator = ' ';
                string[] commandTokens = inputString.Split(separator);
                
                string verb = null;
                string subject = null;
                if (commandTokens.Length == 0)
                {
                    continue;
                }
                else if (commandTokens.Length == 1)
                {
                    verb = commandTokens[0];

                }
                else
                {
                    verb = commandTokens[0];
                    subject = commandTokens[1];
                }

                Commands command = ToCommand(verb);
                string outputString = null;
                switch (command)
                {
                    case Commands.Quit:
                        isRunning = false;
                        outputString = "Thank you for playing!";
                        break;

                    case Commands.Look:
                        Output.WriteLine(Player.CurrentRoom.Description);
                        foreach (Item item in Player.CurrentRoom.Inventory)
                        {
                            Output.WriteLine(item.Description);
                        }
                        outputString = null;
                        break;

                    case Commands.North:
                    case Commands.South:
                    case Commands.East:
                    case Commands.West:
                        Directions direction = (Directions)command;
                        if (Player.Move(direction))
                        {
                            outputString = $"You moved {direction}.\n";
                        }
                        else
                        {
                            outputString = "The way is shut!\n";
                        }
                        break;

                    case Commands.Take:
                        if (subject != null)
                        {
                            Player.Take(subject, this);
                        }
                        else
                        {
                            outputString = "This command requires a subject.\n";
                        };
                        break;

                    case Commands.Drop:
                        if (subject != null)
                        {
                            Player.Drop(subject, this);
                        }
                        else
                        {
                            outputString = "This command requires a subject.\n";
                        }
                        break;

                    case Commands.Inventory:
                        if (Player.Inventory.Count > 0)
                        {
                            Output.WriteLine("\nYou are carrying: ");
                            foreach (Item item in Player.Inventory)
                            {
                                Output.WriteLine(item.Description);
                            }
                            Output.WriteLine("");
                        }
                        else
                        {
                            outputString = "You are empty handed.\n";
                        }
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;
                }
                
                Output.WriteLine(outputString);

            }
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
    }
}
