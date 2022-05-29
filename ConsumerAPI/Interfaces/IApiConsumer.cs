using ConsumerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsumerAPI.Interfaces
{
    interface IApiConsumer
    {
        Task<List<Pessoa>> ConsultarPessoas();
        Task<bool> CriarPessoa(string pessoa);
        Task<Pessoa> EditarPessoa(int idPessoa, string nomePessoa);
        Task<Pessoa> ConsultarPessoaById(int id);
    }
}
