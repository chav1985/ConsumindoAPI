using ConsumerAPI.Helpers;
using ConsumerAPI.Interfaces;
using ConsumerAPI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConsumerAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddHttpClient()
                .AddSingleton<IProcessamento, Processamento>()
                .AddSingleton<IApiConsumer, ApiConsumer>()
                .AddSingleton<IConsoleIO, ConsoleIO>()
                .BuildServiceProvider();

            var processamento = serviceProvider.GetService<IProcessamento>();
            processamento.Iniciar(args);
        }
    }
}
