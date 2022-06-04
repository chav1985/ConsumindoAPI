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
                    Console.Write($"\tSeja bem vindo\n\nEscolha uma das opções abaixo:" +
                        $"\n1 - Consultar Pessoas" +
                        $"\n2 - Criar Pessoa" +
                        $"\n3 - Editar Pessoa" +
                        $"\n4 - Consultar Pessoa por Id" +
                        $"\n5 - Excluir Pessoa" +
                        $"\n0 - Sair\n\n" +
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

                                if (await _apiConsumer.CriarPessoa(new Pessoa { Nome = nomePessoa }))
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

                            case "3":
                                Console.Clear();
                                Console.Write("\tOpção Escolhida - Editar Pessoa\nDigite o Id da Pessoa: ");
                                string idPes = Console.ReadLine();

                                int valorComp;
                                if (int.TryParse(idPes, out valorComp))
                                {
                                    Console.Write("Digite o nome: ");
                                    string nomePes = Console.ReadLine();
                                    Pessoa itemPessoa = await _apiConsumer.EditarPessoa(new Pessoa { Id = int.Parse(idPes), Nome = nomePes });
                                    if (itemPessoa != null)
                                    {
                                        Console.WriteLine("Pessoa Editada.");
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Erro ao Editar a Pessoa.");
                                        Console.ReadKey();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Valor inválido.");
                                    Console.ReadKey();
                                }

                                break;

                            case "4":
                                Console.Clear();
                                Console.Write("\tOpção Escolhida - Consultar Pessoa por Id\nDigite o Id da pessoa: ");
                                string idPessoa = Console.ReadLine();
                                int valorCompara;
                                if (int.TryParse(idPessoa, out valorCompara))
                                {
                                    Pessoa pessoa = await _apiConsumer.ConsultarPessoaById(int.Parse(idPessoa));
                                    if (pessoa != null)
                                    {
                                        Console.WriteLine($"Id: {pessoa.Id}, Nome: {pessoa.Nome}");
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Pessoa com o Id {idPessoa} não cadastrada");
                                        Console.ReadKey();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Valor invalido.");
                                    Console.ReadKey();
                                }
                                break;

                            case "5":
                                Console.Clear();
                                Console.Write("\tOpção Escolhida - Excluir Pessoa\nDigite o Id da pessoa a excluir: ");
                                string idPesExcluir = Console.ReadLine();
                                if (int.TryParse(idPesExcluir, out valorCompara))
                                {
                                    if (await _apiConsumer.ExcluirPessoa(int.Parse(idPesExcluir)))
                                    {
                                        Console.WriteLine($"Pessoa com o Id {idPesExcluir} excluida");
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Pessoa com o Id {idPesExcluir} não cadastrada");
                                        Console.ReadKey();
                                    }
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
