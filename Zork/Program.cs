using System;
using System.Collections.Generic;
using System.IO;

namespace Zork
{
    class Program
    {
        static Program()
        {
            roomMap = new Dictionary<string, Room>();
            foreach (Room room in Rooms)
            {
                roomMap[room.Name] = room;
            }
        }
        private static Room CurrentRoom
        {
            get
            {
                return Rooms[Location.Row, Location.Column];
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            string defaultRoomsFilename = "Rooms.txt";
            string roomsFilename = (args.Length > 0 ? args[(int)CommandLineArguments.RoomsFilename] : defaultRoomsFilename);
            InitializeRoomDescriptions(roomsFilename);


            Room previousRoom = null;
            Commands command = Commands.UNKNOWN;
            while (command != Commands.QUIT)
            {
                Console.WriteLine(CurrentRoom);

                if (previousRoom != CurrentRoom)
                {
                    Console.WriteLine(CurrentRoom.Description);
                    previousRoom = CurrentRoom;
                }

                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());

                switch (command)
                {
                    case Commands.LOOK:
                        Console.WriteLine(CurrentRoom.Description);
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        if (Move(command) == false)
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
                case Commands.NORTH when Location.Row < Rooms.GetLength(0) - 1:
                    Location.Row++;
                    break;

                case Commands.SOUTH when Location.Row > 0:
                    Location.Row--;
                    break;


                case Commands.WEST when Location.Column > 0:
                    Location.Column--;
                    break;

                case Commands.EAST when Location.Column < Rooms.GetLength(1) - 1:
                    Location.Column++;
                    break;

                default:
                    isValidMove = false;
                    break;
            }
            return isValidMove;
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;

        private static readonly Room[,] Rooms =
        {
            {new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            {new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            {new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };

        private static void InitializeRoomDescriptions(string roomsFilename)
        {

            const string fieldDelimiter = "##";
            const int expectedFieldCount = 2;

            string[] lines = File.ReadAllLines(roomsFilename);
            foreach (string line in lines)
            {
                string[] fields = line.Split(fieldDelimiter);
                if (fields.Length != expectedFieldCount)
                {
                    throw new InvalidDataException("Invalid Record.");
                }

                string name = fields[(int)Fields.Name];
                string description = fields[(int)Fields.Description];

                roomMap[name].Description = description;
            }

        }

        private static (int Row, int Column) Location = (1, 1);

        private static readonly Dictionary<string, Room> roomMap;

        private enum Fields
        {
            Name = 0,
            Description
        }

        private enum CommandLineArguments
        {
            RoomsFilename = 0
        }
    }
}