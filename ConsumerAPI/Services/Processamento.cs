using ConsumerAPI.Interfaces;
using ConsumerAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsumerAPI.Services
{
    class Processamento : IProcessamento
    {
        private readonly IApiConsumer _apiConsumer;

        public Processamento(IApiConsumer apiConsumer)
        {
            _apiConsumer = apiConsumer;
        }

        public void Iniciar(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        async Task RunAsync()
        {
            try
            {
                string opcaoEscolhida;

                do
                {
                    Console.Clear();
                    Console.Write($"\tSeja bem vindo\n\nEscolha uma das opções abaixo:\n1 - Consultar Pessoas\n2 - Criar Pessoa\n3 - Editar Pessoa\n4 - Consultar Pessoa por Id\n0 - Sair\n\n" +
                        $"Digite uma opção: ");
                    opcaoEscolhida = Console.ReadLine();
                    int opcaoValida;
                    bool validaOpcao = int.TryParse(opcaoEscolhida, out opcaoValida);

                    if (validaOpcao)
                    {
                        switch (opcaoEscolhida)
                        {
                            case "0":
                                Console.Clear();
                                Console.WriteLine("\tOpção Escolhida - Sair");
                                Console.ReadKey();
                                break;

                            case "1":
                                Console.Clear();
                                Console.Write("\tOpção Escolhida - Consultar Pessoas\n");

                                List<Pessoa> lstPessoas = await _apiConsumer.ConsultarPessoas();
                                if (lstPessoas.Count > 0)
                                {

                                    foreach (var pessoa in lstPessoas)
                                    {
                                        Console.WriteLine($"Id: {pessoa.Id}, Nome: {pessoa.Nome}");
                                    }
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.WriteLine("Sem Pessoas Cadastradas.");
                                    Console.ReadKey();
                                }
                                break;

                            case "2":
                                Console.Clear();
                                Console.Write("\tOpção Escolhida - Criar Pessoa\nDigite o nome: ");
                                string nomePessoa = Console.ReadLine();

                                if (await _apiConsumer.CriarPessoa(nomePessoa))
                                {
                                    Console.WriteLine("Pessoa Cadastrada.");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.WriteLine("Erro ao Cadastrar a Pessoa.");
                                    Console.ReadKey();
                                }
                                break;

                            //case "3":
                            //    Pessoa itemPessoa = await EditarPessoa(new Pessoa());
                            //    if (itemPessoa != null)
                            //    {
                            //        Console.WriteLine("Pessoa Editada.");
                            //        Console.ReadKey();
                            //    }
                            //    else
                            //    {
                            //        Console.WriteLine("Erro ao Editar a Pessoa.");
                            //        Console.ReadKey();
                            //    }
                            //    break;

                            case "4":
                                Console.Clear();
                                Console.Write("\tOpção Escolhida - Consultar Pessoa por Id\nDigite o Id da pessoa: ");
                                string idPessoa = Console.ReadLine();
                                int valorCompara;
                                if (int.TryParse(idPessoa, out valorCompara))
                                {
                                    Pessoa pessoa = await _apiConsumer.ConsultarPessoaById(int.Parse(idPessoa));
                                    Console.WriteLine($"Id: {pessoa.Id}, Nome: {pessoa.Nome}");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.WriteLine("Valor invalido.");
                                    Console.ReadKey();
                                }
                                break;

                            default:
                                Console.WriteLine("Opção Inválida!");
                                Console.ReadKey();
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Opção Inválida!");
                    }
                } while (opcaoEscolhida != "0");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
