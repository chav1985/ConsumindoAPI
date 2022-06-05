using ConsumerAPI.Interfaces;
using ConsumerAPI.Models;
using ConsumerAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using System.Collections.Generic;

namespace ConsumerAPI.Test
{
    [TestClass]
    public class ProcessamentoTests
    {
        private AutoMocker mocker;

        [TestInitialize]
        public void Setup()
        {
            mocker = new AutoMocker();
            mocker.Use(new Mock<IConsoleIO>());
            mocker.Use(new Mock<IApiConsumer>());
        }

        [TestMethod]
        [Description("Validando sair da aplicação")]
        public void IniciarSairAplicacaoTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("0");
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Once);
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [Description("Validando opcao invalida da aplicação")]
        public void IniciarAplicacaoOpcaoInvalidaTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("A").Returns("0");
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(2));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [Description("Validando opcao invalida da aplicação parte 2")]
        public void IniciarAplicacaoOpcaoInvalidaPt2Test()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("6").Returns("0");
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(2));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [Description("Gerando expection na aplicação")]
        public void IniciarAplicacaoExceptionTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().Setup(x => x.ReadLine()).Throws(new System.Exception());
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Once);
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [Description("Validando consultar pessoas com retorno nulo")]
        public void IniciarConsultarPessoasNuloTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("1").Returns("0");
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(3));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [Description("Validando consultar pessoas")]
        public void IniciarConsultarPessoasTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("1").Returns("0");
            mocker.GetMock<IApiConsumer>().Setup(x => x.ConsultarPessoas()).ReturnsAsync(new List<Models.Pessoa> { new Models.Pessoa { Id = 1, Nome = "Teste" } });
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(3));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [Description("Validando criar pessoa")]
        public void IniciarCriarPessoaTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("2").Returns("Teste").Returns("0");
            mocker.GetMock<IApiConsumer>().Setup(x => x.CriarPessoa(It.IsAny<Pessoa>())).ReturnsAsync(true);
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(3));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [Description("Erro ao criar pessoa")]
        public void IniciarCriarPessoaErroTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("2").Returns("Teste").Returns("0");
            mocker.GetMock<IApiConsumer>().Setup(x => x.CriarPessoa(It.IsAny<Pessoa>())).ReturnsAsync(false);
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(3));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
