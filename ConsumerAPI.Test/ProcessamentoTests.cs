using ConsumerAPI.Interfaces;
using ConsumerAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;

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
        }

        [TestMethod]
        [Description("Teste validando sair da aplicação")]
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
    }
}
