using ConsumerAPI.Interfaces;
using ConsumerAPI.Models;
using System;
using System.Collections.Generic;
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
                    int opcaoValida;
                    bool validaOpcao = int.TryParse(opcaoEscolhida, out opcaoValida);

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
            }
        }

        public async Task ConsultarPessoas()
        {
            _console.Clear();
            _console.Write("\tOpção Escolhida - Consultar Pessoas\n");

            List<Pessoa> lstPessoas = await _apiConsumer.ConsultarPessoas();
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

        public async Task CriarPessoa()
        {
            _console.Clear();
            _console.Write("\tOpção Escolhida - Criar Pessoa\nDigite o nome: ");
            string nomePessoa = _console.ReadLine();

            if (await _apiConsumer.CriarPessoa(new Pessoa { Nome = nomePessoa }))
            {
                _console.WriteLine("Pessoa Cadastrada.");
                _console.ReadKey();
            }
            else
            {
                _console.WriteLine("Erro ao Cadastrar a Pessoa.");
                _console.ReadKey();
            }
        }

        public async Task EditarPessoa()
        {
            _console.Clear();
            _console.Write("\tOpção Escolhida - Editar Pessoa\nDigite o Id da Pessoa: ");
            string idPes = _console.ReadLine();

            int valorComp;
            if (int.TryParse(idPes, out valorComp))
            {
                _console.Write("Digite o nome: ");
                string nomePes = _console.ReadLine();
                Pessoa itemPessoa = await _apiConsumer.EditarPessoa(new Pessoa { Id = int.Parse(idPes), Nome = nomePes });
                if (itemPessoa != null)
                {
                    _console.WriteLine("Pessoa Editada.");
                    _console.ReadKey();
                }
                else
                {
                    _console.WriteLine("Erro ao Editar a Pessoa.");
                    _console.ReadKey();
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
            string idPessoa = _console.ReadLine();
            int valorCompara;
            if (int.TryParse(idPessoa, out valorCompara))
            {
                Pessoa pessoa = await _apiConsumer.ConsultarPessoaById(int.Parse(idPessoa));
                if (pessoa != null)
                {
                    _console.WriteLine($"Id: {pessoa.Id}, Nome: {pessoa.Nome}");
                    _console.ReadKey();
                }
                else
                {
                    _console.WriteLine($"Pessoa com o Id {idPessoa} não cadastrada");
                    _console.ReadKey();
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
            string idPesExcluir = _console.ReadLine();
            int valorCompara;
            if (int.TryParse(idPesExcluir, out valorCompara))
            {
                if (await _apiConsumer.ExcluirPessoa(int.Parse(idPesExcluir)))
                {
                    _console.WriteLine($"Pessoa com o Id {idPesExcluir} excluida");
                    _console.ReadKey();
                }
                else
                {
                    _console.WriteLine($"Pessoa com o Id {idPesExcluir} não cadastrada");
                    _console.ReadKey();
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
    }
}