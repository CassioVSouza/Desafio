using Backend.DTOs;
using Backend.Models;
using Backend.Repositorio.Interfaces;
using Backend.Servicos.Interfaces;
using Backend.Servicos.Principal;
using Moq;

namespace Backend.Tests.Servicos.Principal
{
    public class AmostraServicoTest
    {
        private readonly Mock<ILogServico> _mockLogServico;
        private readonly AmostraServico _amostraServico;
        private readonly Mock<IAmostraRepositorio> _amostraRepo;

        public AmostraServicoTest()
        {
            _mockLogServico = new Mock<ILogServico>();
            _amostraRepo = new Mock<IAmostraRepositorio>();
            _amostraServico = new AmostraServico(_mockLogServico.Object, _amostraRepo.Object);
        }

        [Fact]
        public void ValidarInformacoes_RetornarAmostraComCodigoNovo()
        {

            //Arrange
            var amostraRequest = new AmostraDTO()
            {
                Descricao = "Amostra de Teste",
                DataRecebimento = DateTime.Now,
                Status = "Ativo"
            };

            //Act

            var resultado = _amostraServico.ValidarInformacoes(amostraRequest, false);

            //Asserts
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.Codigo);
        }

        [Fact]
        public void ValidarInformacoes_RetornarAmostraComCodigoIgual()
        {

            //Arrange
            var amostraRequest = new AmostraDTO()
            {
                Codigo = Guid.NewGuid().ToString(),
                Descricao = "Amostra de Teste",
                DataRecebimento = DateTime.Now,
                Status = "Ativo"
            };

            //Act

            var resultado = _amostraServico.ValidarInformacoes(amostraRequest, true);

            //Asserts
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.Codigo);
            Assert.Equal(amostraRequest.Codigo, resultado.Codigo);
        }
    }
}
