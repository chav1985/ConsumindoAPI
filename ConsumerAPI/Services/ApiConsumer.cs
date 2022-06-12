using ConsumerAPI.Interfaces;
using ConsumerAPI.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ConsumerAPI.Services
{
    public class ApiConsumer : IApiConsumer
    {
        private readonly HttpClient _httpClient;
        private readonly ResponseMessage _responseMessage;

        public ApiConsumer(HttpClient httpClient, ResponseMessage responseMessage)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5010");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _responseMessage = responseMessage;
        }

        public async Task<ResponseMessage> ConsultarPessoas()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/pessoa");
            _responseMessage.StatusId = response.StatusCode;
            _responseMessage.Content = await response.Content.ReadAsStringAsync();
            return _responseMessage;
        }

        public async Task<ResponseMessage> CriarPessoa(Pessoa pessoa)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/pessoa", pessoa);
            _responseMessage.StatusId = response.StatusCode;
            _responseMessage.Content = await response.Content.ReadAsStringAsync();
            return _responseMessage;
        }

        public async Task<ResponseMessage> EditarPessoa(Pessoa pessoa)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
                $"api/pessoa/{pessoa.Id}", pessoa);
            _responseMessage.StatusId = response.StatusCode;
            _responseMessage.Content = await response.Content.ReadAsStringAsync();
            return _responseMessage;
        }

        public async Task<ResponseMessage> ConsultarPessoaById(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/pessoa/{id}");
            _responseMessage.StatusId = response.StatusCode;
            _responseMessage.Content = await response.Content.ReadAsStringAsync();
            return _responseMessage;
        }

        public async Task<ResponseMessage> ExcluirPessoa(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/pessoa/{id}");
            _responseMessage.StatusId = response.StatusCode;
            _responseMessage.Content = await response.Content.ReadAsStringAsync();
            return _responseMessage;
        }
    }
}
