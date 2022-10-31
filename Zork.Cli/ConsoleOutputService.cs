using System;
using Zork.Common;

namespace Zork.Cli
{
    internal class ConsoleOutputService : IOutputService
    {
        public void Write(object obj)
        {
            Console.Write(obj);
        }

        public void Write(string message)
        {
            Console.Write(message);
        }

        public void WriteLine(object obj)
        {
            Console.WriteLine(obj);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}