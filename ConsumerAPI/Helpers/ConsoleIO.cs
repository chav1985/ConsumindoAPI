using ConsumerAPI.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ConsumerAPI.Helpers
{
    [ExcludeFromCodeCoverage]
    public class ConsoleIO : IConsoleIO
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
