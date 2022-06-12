using ConsumerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsumerAPI.Interfaces
{
    public interface IApiConsumer
    {
        Task<ResponseMessage> ConsultarPessoas();
        Task<ResponseMessage> CriarPessoa(Pessoa pessoa);
        Task<ResponseMessage> EditarPessoa(Pessoa pessoa);
        Task<ResponseMessage> ConsultarPessoaById(int id);
        Task<ResponseMessage> ExcluirPessoa(int id);
    }
}
