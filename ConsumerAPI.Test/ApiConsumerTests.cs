using ConsumerAPI.Models;
using ConsumerAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsumerAPI.Test
{
    [TestClass]
    public class ApiConsumerTests
    {
        private MockHttpMessageHandler mockHttp;
        private List<Pessoa> lstPessoas;
        private HttpClient http;
        private ApiConsumer api;
        private ResponseMessage response;

        [TestInitialize]
        public void Setup()
        {
            mockHttp = new MockHttpMessageHandler();
            lstPessoas = new List<Pessoa>();
            response = new ResponseMessage();
        }

        [TestMethod]
        [Description("Validando consultar pessoas")]
        public async Task ConsultarPessoasTest()
        {
            //arrange
            lstPessoas.Add(new Pessoa { Id = 1, Nome = "Teste" });
            string jsonPessoas = JsonConvert.SerializeObject(lstPessoas);
            mockHttp.When("https://localhost:5010/api/*").Respond("application/json", jsonPessoas);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http, response);

            //act
            var validaRetorno = await api.ConsultarPessoas();

            //assert
            var objDeserialize = JsonConvert.DeserializeObject<List<Pessoa>>(validaRetorno.Content);
            Assert.AreEqual(1, objDeserialize.Count);
        }

        [TestMethod]
        [Description("Validando consultar pessoas retornando nulo")]
        public async Task ConsultarPessoasNuloTest()
        {
            //arrange
            string jsonPessoas = JsonConvert.SerializeObject(lstPessoas);
            mockHttp.When("https://localhost:5010/api/*").Respond(HttpStatusCode.NotFound);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http, response);

            //act
            var validaRetorno = await api.ConsultarPessoas();

            //assert
            Assert.AreEqual(HttpStatusCode.NotFound, validaRetorno.StatusId);
        }

        [TestMethod]
        [Description("Validando criar pessoa")]
        public async Task CriarPessoaTest()
        {
            //arrange
            string pessoa = "{\"Id\": 1, \"Nome\": \"Teste123\"}";
            mockHttp.When("https://localhost:5010/api/*").Respond("application/json", pessoa);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http, response);

            //act
            var validaRetorno = await api.CriarPessoa(new Pessoa { Id = 1, Nome = "Teste" });

            //assert
            var objDeserialize = JsonConvert.DeserializeObject<Pessoa>(validaRetorno.Content);
            Assert.AreEqual("Teste123", objDeserialize.Nome);
        }

        [TestMethod]
        [Description("Erro ao criar pessoa")]
        public async Task CriarPessoaErroTest()
        {
            //arrange
            mockHttp.When("https://localhost:5010/api/*").Respond(HttpStatusCode.BadRequest);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http, response);

            //act
            var validaRetorno = await api.CriarPessoa(new Pessoa { Id = 1, Nome = "Teste" });

            //assert
            Assert.AreEqual(HttpStatusCode.BadRequest, validaRetorno.StatusId);
        }

        [TestMethod]
        [Description("Validando editar pessoa")]
        public async Task EditarPessoaTest()
        {
            //arrange
            string pessoa = "{\"Id\": 1, \"Nome\": \"Teste321\"}";
            mockHttp.When("https://localhost:5010/api/*").Respond("application/json", pessoa);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http, response);

            //act
            var validaRetorno = await api.EditarPessoa(new Pessoa());

            //assert
            var objDeserialize = JsonConvert.DeserializeObject<Pessoa>(validaRetorno.Content);
            Assert.AreEqual("Teste321", objDeserialize.Nome);
        }

        [TestMethod]
        [Description("Erro ao editar pessoa")]
        public async Task EditarPessoaErroTest()
        {
            //arrange
            mockHttp.When("https://localhost:5010/api/*").Respond(HttpStatusCode.NotFound);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http, response);

            //act
            var validaRetorno = await api.EditarPessoa(new Pessoa());

            //assert
            Assert.AreEqual(HttpStatusCode.NotFound, validaRetorno.StatusId);
        }

        [TestMethod]
        [Description("Validando consultar pessoa por id")]
        public async Task ConsultarPessoaByIdTest()
        {
            //arrange
            string pessoa = "{\"Id\": 1, \"Nome\": \"Teste123\"}";
            mockHttp.When("https://localhost:5010/api/*").Respond("application/json", pessoa);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http, response);

            //act
            var validaRetorno = await api.ConsultarPessoaById(1);

            //assert
            var objDeserialize = JsonConvert.DeserializeObject<Pessoa>(validaRetorno.Content);
            Assert.AreEqual("Teste123", objDeserialize.Nome);
        }

        [TestMethod]
        [Description("Erro ao consultar pessoa por id")]
        public async Task ConsultarPessoaByIdErroTest()
        {
            //arrange
            mockHttp.When("https://localhost:5010/api/*").Respond(HttpStatusCode.NotFound);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http, response);

            //act
            var validaRetorno = await api.ConsultarPessoaById(1);

            //assert
            Assert.AreEqual(HttpStatusCode.NotFound, validaRetorno.StatusId);
        }

        [TestMethod]
        [Description("Validando excluir pessoa")]
        public async Task ExcluirPessoaTest()
        {
            //arrange
            string pessoa = "{\"Id\": 1, \"Nome\": \"Teste123\"}";
            mockHttp.When("https://localhost:5010/api/*").Respond("application/json", pessoa);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http, response);

            //act
            var validaRetorno = await api.ExcluirPessoa(1);

            //assert
            var objDeserialize = JsonConvert.DeserializeObject<Pessoa>(validaRetorno.Content);
            Assert.AreEqual("Teste123", objDeserialize.Nome);
        }

        [TestMethod]
        [Description("Erro ao excluir pessoa")]
        public async Task ExcluirPessoaErroTest()
        {
            //arrange
            mockHttp.When("https://localhost:5010/api/*").Respond(HttpStatusCode.NotFound);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http, response);

            //act
            var validaRetorno = await api.ExcluirPessoa(1);

            //assert
            Assert.AreEqual(HttpStatusCode.NotFound, validaRetorno.StatusId);
        }
    }
}