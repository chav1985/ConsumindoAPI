using ConsumerAPI.Interfaces;
using System;

namespace ConsumerAPI.Helpers
{
    internal class ConsoleIO : IConsoleIO
    {
        public void Clear()
        {
            Console.Clear();
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void Write(string texto)
        {
            Console.Write(texto);
        }

        public void WriteLine(string texto)
        {
            Console.WriteLine(texto);
        }
    }
}
