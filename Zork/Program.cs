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
                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());

                string outputString;
                switch (command)
                {
                    case Commands.LOOK:
                        outputString = "This is an open field west of a white house, with a boarded front door.\nA rubber mat saying 'Welcome to Zork!' lies by the door";
                        break;

                    case Commands.NORTH:
                        outputString = "You moved NORTH.";
                        break;

                    case Commands.SOUTH:
                        outputString = "You moved SOUTH.";
                        break;

                    case Commands.EAST:
                        outputString = "You moved EAST.";
                        break;

                    case Commands.WEST:
                        outputString = "You moved WEST.";
                        break;

                    case Commands.QUIT:
                        outputString = "Thanks for playing.";
                        break;

                    default:
                        outputString = "Unknown Command";
                        break;
                }

                Console.WriteLine(outputString);
            }
            

        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
        
    }
}
