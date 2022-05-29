using ConsumerAPI.Interfaces;
using ConsumerAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ConsumerAPI.Services
{
    class ApiConsumer : IApiConsumer
    {
        private readonly HttpClient _httpClient;

        public ApiConsumer(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5001/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Pessoa>> ConsultarPessoas()
        {
            List<Pessoa> pessoas = new List<Pessoa>();

            HttpResponseMessage response = await _httpClient.GetAsync("pessoa");
            if (response.IsSuccessStatusCode)
            {
                pessoas = await response.Content.ReadFromJsonAsync<List<Pessoa>>();
            }

            return pessoas;
        }

        public async Task<bool> CriarPessoa(string nomePessoa)
        {
            Pessoa pessoa = new Pessoa { Nome = nomePessoa };
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("pessoa", pessoa);
            return response.IsSuccessStatusCode;
        }

        public async Task<Pessoa> EditarPessoa(int idPessoa, string nomePessoa)
        {
            Pessoa pessoa = new Pessoa { Id = idPessoa, Nome = nomePessoa };
            //Console.Clear();
            //Console.Write("\tOpção Escolhida - Editar Pessoa\nDigite o Id da Pessoa: ");
            //string idPessoa = Console.ReadLine();
            //pessoa.Id = Convert.ToInt32(idPessoa);
            //Console.Write("\nDigite o nome: ");
            //string nomePessoa = Console.ReadLine();
            //pessoa.Nome = nomePessoa;

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
                $"pessoa/{pessoa.Id}", pessoa);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the updated product from the response body.
                pessoa = await response.Content.ReadFromJsonAsync<Pessoa>();
                return pessoa;

            }
            return null;
        }

        public async Task<Pessoa> ConsultarPessoaById(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"pessoa/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Pessoa>();
            }

            return null;
        }
    }
}
