using ConsumerAPI.Interfaces;
using ConsumerAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ConsumerAPI.Services
{
    public class Processamento : IProcessamento
    {
        private readonly IApiConsumer _apiConsumer;
        private readonly IConsoleIO _console;

        public Processamento(IApiConsumer apiConsumer, IConsoleIO console)
        {
            _apiConsumer = apiConsumer;
            _console = console;
        }

        public void Iniciar(string[] args)
        {
            try
            {
                string opcaoEscolhida;

                do
                {
                    _console.Clear();
                    _console.Write($"\tSeja bem vindo\n\nEscolha uma das opções abaixo:" +
                        $"\n1 - Consultar Pessoas" +
                        $"\n2 - Criar Pessoa" +
                        $"\n3 - Editar Pessoa" +
                        $"\n4 - Consultar Pessoa por Id" +
                        $"\n5 - Excluir Pessoa" +
                        $"\n0 - Sair\n\n" +
                        $"Digite uma opção: ");
                    opcaoEscolhida = _console.ReadLine();
                    bool validaOpcao = ValidaInteiro(opcaoEscolhida);

                    if (validaOpcao)
                    {
                        switch (opcaoEscolhida)
                        {
                            case "0":
                                SairAplicacao();
                                break;

                            case "1":
                                ConsultarPessoas().Wait();
                                break;

                            case "2":
                                CriarPessoa().Wait();
                                break;

                            case "3":
                                EditarPessoa().Wait();
                                break;

                            case "4":
                                ConsultarPessoaId().Wait();
                                break;

                            case "5":
                                ExcluirPessoa().Wait();
                                break;

                            default:
                                _console.WriteLine("Opção Inválida!");
                                _console.ReadKey();
                                break;
                        }
                    }
                    else
                    {
                        _console.WriteLine("Opção Inválida!");
                        _console.ReadKey();
                    }
                } while (opcaoEscolhida != "0");
            }
            catch (Exception e)
            {
                _console.WriteLine($"Erro na aplicação: {e.Message}");
                _console.ReadKey();
            }
        }

        public async Task ConsultarPessoas()
        {
            _console.Clear();
            _console.Write("\tOpção Escolhida - Consultar Pessoas\n");

            var retornoApi = await _apiConsumer.ConsultarPessoas();

            if (retornoApi.StatusId == HttpStatusCode.OK)
            {
                var lstPessoas = JsonConvert.DeserializeObject<List<Pessoa>>(retornoApi.Content);

                if (lstPessoas is not null && lstPessoas.Count > 0)
                {
                    foreach (var pessoa in lstPessoas)
                    {
                        _console.WriteLine($"Id: {pessoa.Id}, Nome: {pessoa.Nome}");
                    }
                    _console.ReadKey();
                }
                else
                {
                    _console.WriteLine("Sem Pessoas Cadastradas.");
                    _console.ReadKey();
                }
            }
            else
            {
                TratarStatusCode(retornoApi);
            }
        }

        public async Task CriarPessoa()
        {
            _console.Clear();
            _console.Write("\tOpção Escolhida - Criar Pessoa\nDigite o nome: ");
            string nomePes = _console.ReadLine();

            var retornoApi = await _apiConsumer.CriarPessoa(new Pessoa { Nome = nomePes });

            if (retornoApi.StatusId == HttpStatusCode.OK)
            {
                var pessoa = JsonConvert.DeserializeObject<Pessoa>(retornoApi.Content);
                _console.WriteLine($"{pessoa.Nome} Cadastrado.");
                _console.ReadKey();
            }
            else
            {
                TratarStatusCode(retornoApi);
            }
        }

        public async Task EditarPessoa()
        {
            _console.Clear();
            _console.Write("\tOpção Escolhida - Editar Pessoa\nDigite o Id da Pessoa: ");
            string idPes = _console.ReadLine();

            if (ValidaInteiro(idPes))
            {
                _console.Write("Digite o nome: ");
                string nomePes = _console.ReadLine();

                var retornoApi = await _apiConsumer.EditarPessoa(new Pessoa { Id = int.Parse(idPes), Nome = nomePes });

                if (retornoApi.StatusId == HttpStatusCode.OK)
                {
                    _console.WriteLine("Pessoa Editada.");
                    _console.ReadKey();
                }
                else
                {
                    TratarStatusCode(retornoApi);
                }
            }
            else
            {
                _console.WriteLine("Valor inválido.");
                _console.ReadKey();
            }
        }

        public async Task ConsultarPessoaId()
        {
            _console.Clear();
            _console.Write("\tOpção Escolhida - Consultar Pessoa por Id\nDigite o Id da pessoa: ");
            string idPes = _console.ReadLine();

            if (ValidaInteiro(idPes))
            {
                var retornoApi = await _apiConsumer.ConsultarPessoaById(int.Parse(idPes));

                if (retornoApi.StatusId == HttpStatusCode.OK)
                {
                    var pessoa = JsonConvert.DeserializeObject<Pessoa>(retornoApi.Content);
                    _console.WriteLine($"Id: {pessoa.Id}, Nome: {pessoa.Nome}");
                    _console.ReadKey();
                }
                else
                {
                    TratarStatusCode(retornoApi);
                }
            }
            else
            {
                _console.WriteLine("Valor invalido.");
                _console.ReadKey();
            }
        }

        public async Task ExcluirPessoa()
        {
            _console.Clear();
            _console.Write("\tOpção Escolhida - Excluir Pessoa\nDigite o Id da pessoa a excluir: ");
            string idPes = _console.ReadLine();

            if (ValidaInteiro(idPes))
            {
                var retornoApi = await _apiConsumer.ExcluirPessoa(int.Parse(idPes));

                if (retornoApi.StatusId == HttpStatusCode.OK)
                {
                    _console.WriteLine($"Pessoa com o Id {idPes} excluida.");
                    _console.ReadKey();
                }
                else
                {
                    TratarStatusCode(retornoApi);
                }
            }
            else
            {
                _console.WriteLine("Valor invalido.");
                _console.ReadKey();
            }
        }

        public void SairAplicacao()
        {
            _console.Clear();
            _console.WriteLine("\tOpção Escolhida - Sair");
            _console.ReadKey();
        }

        private bool ValidaInteiro(string valor)
        {
            int valorCompara;
            return int.TryParse(valor, out valorCompara);
        }

        private void TratarStatusCode(ResponseMessage response)
        {
            switch (response.StatusId)
            {
                case HttpStatusCode.NotFound:
                    _console.WriteLine("Objeto não localizado.");
                    break;
                case HttpStatusCode.BadRequest:
                    var stringJson = JsonConvert.DeserializeObject(response.Content);
                    _console.WriteLine($"Erros na resposta da requisição: \n{stringJson}");
                    break;
                default:
                    _console.WriteLine("Erro generico, sem tratamento.");
                    break;
            }
            _console.ReadKey();
        }
    }
}