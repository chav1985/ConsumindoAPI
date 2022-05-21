using ConsumerAPI.Services;
using System;

namespace ConsumerAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Processamento processamento = new Processamento();
            processamento.Iniciar(args);
            Console.WriteLine("Hello World!");
        }
    }
}
