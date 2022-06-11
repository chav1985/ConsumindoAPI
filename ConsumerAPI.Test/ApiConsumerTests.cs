using ConsumerAPI.Models;
using ConsumerAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsumerAPI.Test
{
    [TestClass]
    public class ApiConsumerTests
    {
        private MockHttpMessageHandler mockHttp;
        private List<Pessoa> lstPessoas;

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
            HttpClient http = new HttpClient(mockHttp);
            ApiConsumer api = new ApiConsumer(http);

            //act
            var retornoMetodo = await api.ConsultarPessoas();

            //assert
            Assert.AreEqual(1, retornoMetodo.Count);
        }

        [TestMethod]
        [Description("Validando consultar pessoas retornando nulo")]
        public async Task ConsultarPessoasNuloTest()
        {
            //arrange
            string jsonPessoas = JsonConvert.SerializeObject(lstPessoas);
            mockHttp.When("https://localhost:5010/api/TESTE").Respond("application/json", jsonPessoas);
            HttpClient http = new HttpClient(mockHttp);
            ApiConsumer api = new ApiConsumer(http);

            //act
            var retornoMetodo = await api.ConsultarPessoas();

            //assert
            Assert.IsNull(retornoMetodo);
        }
    }
}
