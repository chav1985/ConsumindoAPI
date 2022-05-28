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
                .AddSingleton<IProcessamento, Processamento>()
                .BuildServiceProvider();

            var processamento = serviceProvider.GetService<IProcessamento>();
            processamento.Iniciar(args);
        }
    }
}
