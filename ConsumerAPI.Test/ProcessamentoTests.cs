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

        [TestMethod]
        [Description("Validando editar pessoa")]
        public void IniciarEditarPessoaTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("3").Returns("1").Returns("Teste").Returns("0");
            mocker.GetMock<IApiConsumer>().Setup(x => x.EditarPessoa(It.IsAny<Pessoa>())).ReturnsAsync(new Pessoa());
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(4));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [Description("Erro ao editar pessoa")]
        public void IniciarEditarPessoaErroTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("3").Returns("1").Returns("Teste").Returns("0");
            mocker.GetMock<IApiConsumer>().Setup(x => x.EditarPessoa(It.IsAny<Pessoa>())).ReturnsAsync((Pessoa)null);
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(4));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [Description("Validando editar pessoa opcao invalida")]
        public void IniciarEditarPessoaOpcaoInvalidaTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("3").Returns("A").Returns("0");
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(3));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [Description("Validando consultar pessoa por id")]
        public void IniciarCOnsultarPessoaPorIdTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("4").Returns("1").Returns("0");
            mocker.GetMock<IApiConsumer>().Setup(x => x.ConsultarPessoaById(It.IsAny<int>())).ReturnsAsync(new Pessoa());
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(3));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [Description("Erro ao consultar pessoa por id")]
        public void IniciarConsultarPessoaPorIdErroTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("4").Returns("1").Returns("0");
            mocker.GetMock<IApiConsumer>().Setup(x => x.ConsultarPessoaById(It.IsAny<int>())).ReturnsAsync((Pessoa)null);
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(3));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [Description("Validando consultar pessoa por id opcao invalida")]
        public void IniciarConsultarPessoaPorIdOpcaoInvalidaTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("4").Returns("A").Returns("0");
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(3));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [Description("Validando excluir pessoa")]
        public void IniciarExcluirPessoaTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("5").Returns("1").Returns("0");
            mocker.GetMock<IApiConsumer>().Setup(x => x.ExcluirPessoa(It.IsAny<int>())).ReturnsAsync(true);
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(3));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [Description("Erro ao excluir pessoa")]
        public void IniciarExcluirPessoaErroTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("5").Returns("1").Returns("0");
            mocker.GetMock<IApiConsumer>().Setup(x => x.ExcluirPessoa(It.IsAny<int>())).ReturnsAsync(false);
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(3));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [Description("Validando excluir pessoa opcao invalida")]
        public void IniciarExcluirPessoaOpcaoInvalidaTest()
        {
            //arrange
            mocker.GetMock<IConsoleIO>().SetupSequence(x => x.ReadLine()).Returns("5").Returns("A").Returns("0");
            var processamento = mocker.CreateInstance<Processamento>();

            //act
            processamento.Iniciar(null);

            //assert
            mocker.GetMock<IConsoleIO>().Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(3));
            mocker.GetMock<IConsoleIO>().Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
