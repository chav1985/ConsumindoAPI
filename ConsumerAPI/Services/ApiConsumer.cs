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
            _httpClient.BaseAddress = new Uri("https://localhost:5001");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Pessoa>> ConsultarPessoas()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/pessoa");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await response.Content.ReadFromJsonAsync<List<Pessoa>>();
            }

            return null;
        }

        public async Task<bool> CriarPessoa(Pessoa pessoa)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/pessoa", pessoa);
            return response.IsSuccessStatusCode;
        }

        public async Task<Pessoa> EditarPessoa(Pessoa pessoa)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
                $"api/pessoa/{pessoa.Id}", pessoa);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the updated product from the response body.
                return await response.Content.ReadFromJsonAsync<Pessoa>();

            }
            return null;
        }

        public async Task<Pessoa> ConsultarPessoaById(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/pessoa/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Pessoa>();
            }

            return null;
        }

        public async Task<bool> ExcluirPessoa(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/pessoa/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
