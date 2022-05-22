using ConsumerAPI.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ConsumerAPI.Services
{
    class Processamento
    {
        static HttpClient client;

        public void Iniciar(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            client = new HttpClient(clientHandler);

            //client.BaseAddress = new Uri("https://localhost:44311/api/pessoa");
            client.BaseAddress = new Uri("https://localhost:5001/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

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
                                List<Pessoa> lstPessoas = await ConsultarPessoas();
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
                                if (await CriarPessoa(new Pessoa()))
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
                                Pessoa itemPessoa = await EditarPessoa(new Pessoa());
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
                                break;

                            case "4":
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

            Console.ReadKey();
        }

        static async Task<List<Pessoa>> ConsultarPessoas()
        {
            List<Pessoa> pessoas = new List<Pessoa>();

            HttpResponseMessage response = await client.GetAsync("pessoa");
            if (response.IsSuccessStatusCode)
            {
                pessoas = await response.Content.ReadFromJsonAsync<List<Pessoa>>();
            }
            Console.Clear();
            Console.Write("\tOpção Escolhida - Consultar Pessoas\n");
            return pessoas;
        }

        static async Task<bool> CriarPessoa(Pessoa pessoa)
        {
            Console.Clear();
            Console.Write("\tOpção Escolhida - Criar Pessoa\nDigite o nome: ");
            string nomePessoa = Console.ReadLine();
            pessoa.Nome = nomePessoa;
            HttpResponseMessage response = await client.PostAsJsonAsync("pessoa", pessoa);
            return response.IsSuccessStatusCode;
        }

        static async Task<Pessoa> EditarPessoa(Pessoa pessoa)
        {
            Console.Clear();
            Console.Write("\tOpção Escolhida - Editar Pessoa\nDigite o Id da Pessoa: ");
            string idPessoa = Console.ReadLine();
            pessoa.Id = Convert.ToInt32(idPessoa);
            Console.Write("\nDigite o nome: ");
            string nomePessoa = Console.ReadLine();
            pessoa.Nome = nomePessoa;
            
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"pessoa/{pessoa.Id}", pessoa);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            pessoa = await response.Content.ReadFromJsonAsync<Pessoa>();
            return pessoa;
        }
    }
}
