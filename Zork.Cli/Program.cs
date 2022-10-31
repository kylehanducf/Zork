using System;
using System.IO;
using Newtonsoft.Json;
using Zork.Common;

namespace Zork.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            const string defaultRoomsFilename = @"Content\Game.json";
            string gameFilename = (args.Length > 0 ? args[(int)CommandLineArguments.GameFilename] : defaultRoomsFilename);
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(gameFilename));

            var output = new ConsoleOutputService();

            Console.WriteLine("Welcome to Zork!");
            game.Run(output);
            Console.WriteLine("Finished.");
        }

        private enum CommandLineArguments
        {
            GameFilename = 0
        }
    }
}