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

        [TestInitialize]
        public void Setup()
        {
            mockHttp = new MockHttpMessageHandler();
            lstPessoas = new List<Pessoa>();
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
            api = new ApiConsumer(http);

            //act
            var validaRetorno = await api.ConsultarPessoas();

            //assert
            Assert.AreEqual(1, validaRetorno.Count);
        }

        [TestMethod]
        [Description("Validando consultar pessoas retornando nulo")]
        public async Task ConsultarPessoasNuloTest()
        {
            //arrange
            string jsonPessoas = JsonConvert.SerializeObject(lstPessoas);
            mockHttp.When("https://localhost:5010/api/*").Respond(HttpStatusCode.NotFound);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http);

            //act
            var validaRetorno = await api.ConsultarPessoas();

            //assert
            Assert.IsNull(validaRetorno);
        }

        [TestMethod]
        [Description("Validando criar pessoa")]
        public async Task CriarPessoaTest()
        {
            //arrange
            mockHttp.When("https://localhost:5010/api/*").Respond(HttpStatusCode.OK);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http);

            //act
            var validaRetorno = await api.CriarPessoa(new Pessoa { Id = 1, Nome = "Teste" });

            //assert
            Assert.AreEqual(true, validaRetorno);
        }

        [TestMethod]
        [Description("Erro ao criar pessoa")]
        public async Task CriarPessoaErroTest()
        {
            //arrange
            mockHttp.When("https://localhost:5010/api/*").Respond(HttpStatusCode.BadRequest);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http);

            //act
            var validaRetorno = await api.CriarPessoa(new Pessoa { Id = 1, Nome = "Teste" });

            //assert
            Assert.AreEqual(false, validaRetorno);
        }

        [TestMethod]
        [Description("Validando editar pessoa")]
        public async Task EditarPessoaTest()
        {
            //arrange
            string pessoa = "{\"Id\": 1, \"Nome\": \"Teste\"}";
            mockHttp.When("https://localhost:5010/api/*").Respond("application/json", pessoa);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http);

            //act
            var validaRetorno = await api.EditarPessoa(new Pessoa());

            //assert
            Assert.AreEqual("Teste", validaRetorno.Nome);
        }

        [TestMethod]
        [Description("Erro ao editar pessoa")]
        public async Task EditarPessoaErroTest()
        {
            //arrange
            mockHttp.When("https://localhost:5010/api/*").Respond(HttpStatusCode.NotFound);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http);

            //act
            var validaRetorno = await api.EditarPessoa(new Pessoa());

            //assert
            Assert.IsNull(validaRetorno);
        }

        [TestMethod]
        [Description("Validando consultar pessoa por id")]
        public async Task ConsultarPessoaByIdTest()
        {
            //arrange
            string pessoa = "{\"Id\": 1, \"Nome\": \"Teste123\"}";
            mockHttp.When("https://localhost:5010/api/*").Respond("application/json", pessoa);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http);

            //act
            var validaRetorno = await api.ConsultarPessoaById(1);

            //assert
            Assert.AreEqual("Teste123", validaRetorno.Nome);
        }

        [TestMethod]
        [Description("Erro ao consultar pessoa por id")]
        public async Task ConsultarPessoaByIdErroTest()
        {
            //arrange
            //string pessoa = "{\"Id\": 1, \"Nome\": \"Teste123\"}";
            mockHttp.When("https://localhost:5010/api/*").Respond(HttpStatusCode.NotFound);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http);

            //act
            var validaRetorno = await api.ConsultarPessoaById(1);

            //assert
            Assert.IsNull(validaRetorno);
        }

        [TestMethod]
        [Description("Validando excluir pessoa")]
        public async Task ExcluirPessoaTest()
        {
            //arrange
            mockHttp.When("https://localhost:5010/api/*").Respond(HttpStatusCode.OK);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http);

            //act
            var validaRetorno = await api.ExcluirPessoa(1);

            //assert
            Assert.AreEqual(true, validaRetorno);
        }

        [TestMethod]
        [Description("Erro ao excluir pessoa")]
        public async Task ExcluirPessoaErroTest()
        {
            //arrange
            mockHttp.When("https://localhost:5010/api/*").Respond(HttpStatusCode.NotFound);
            http = new HttpClient(mockHttp);
            api = new ApiConsumer(http);

            //act
            var validaRetorno = await api.ExcluirPessoa(1);

            //assert
            Assert.AreEqual(false, validaRetorno);
        }
    }
}
