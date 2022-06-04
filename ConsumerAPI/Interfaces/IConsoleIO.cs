using System;

namespace ConsumerAPI.Interfaces
{
    public interface IConsoleIO
    {
        void Write(string texto);
        void WriteLine(string texto);
        string ReadLine();
        ConsoleKeyInfo ReadKey();
        void Clear();
    }
}
