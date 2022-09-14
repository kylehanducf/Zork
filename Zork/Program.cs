using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");


            Commands command = Commands.UNKNOWN;
            while (command != Commands.QUIT)
            {
                Console.WriteLine(Rooms[CurrentRoom]);
                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());

                switch (command)
                {
                    case Commands.LOOK:
                        Console.WriteLine("This is an open field west of a white house, with a boarded front door.\nA rubber mat saying 'Welcome to Zork!' lies by the door");
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        if(Move(command) == false)
                        {
                            Console.WriteLine("The way is shut!");
                        }
                        else
                        {
                            Console.WriteLine($"You moved {command}.");
                        }
                        break;
                        
                    case Commands.QUIT:
                        Console.WriteLine("Thank you for playing!");
                        break;

                    default:
                        Console.WriteLine("Unknown Command");
                        break;
                }

            }
            

        }

        private static bool Move(Commands command)
        {
            bool isValidMove = true;
            switch (command)
            {
                case Commands.WEST when CurrentRoom > 0:
                    CurrentRoom--;
                    break;

                case Commands.EAST when CurrentRoom < Rooms.Length - 1:
                    CurrentRoom++;
                    break;

                default :
                    isValidMove = false;
                    break;
            }
            return isValidMove;
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;

        private static readonly string[] Rooms = { "Forest", "West of House", "Behind House", "Clearing", "Canyon View" };
        private static int CurrentRoom = 1;

    }
}
