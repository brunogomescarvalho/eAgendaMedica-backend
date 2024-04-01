using EAgendaMedica.Dominio;
using EAgendaMedica.Dominio.Copartilhado;
using EAgendaMedica.Dominio.ModuloMedico;
using EAgendaMedica.Aplicacao.ModuloMedico;
using Moq;
using FluentResults.Extensions.FluentAssertions;
using FluentAssertions;

namespace EAgendaMedica.AplicacaoUnitTests
{
    [TestClass]
    public class ServicoMedicoUnitTests
    {
        Mock<IRepositorioMedico> repositorioMoq;

        ServicoMedico servicoMedico;

        Mock<IContextoPersistencia> contexto;

        public ServicoMedicoUnitTests()
        {
            repositorioMoq = new Mock<IRepositorioMedico>();

            contexto = new Mock<IContextoPersistencia>();
        
            servicoMedico = new ServicoMedico(repositorioMoq.Object, contexto.Object);
        }


        [TestMethod]
        public void Deve_Cadastrar_Medico_Se_Ele_for_Valido()
        {
            var medico = new Medico("medico", "12345-SC");

            var result = servicoMedico.Inserir(medico);

            result.Result.Should().BeSuccess();
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Medico_Se_CRM_for_invalido()
        {
            var medico = new Medico("medico", "12-SC");

            var result = servicoMedico.Inserir(medico);

            result.Result.Should().BeFailure();
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Medico_Se_Nome_for_invalido()
        {
            var medico = new Medico("m", "12345-SC");

            var result = servicoMedico.Inserir(medico);

            result.Result.Should().BeFailure();
        }

        [TestMethod]
        public void Deve_Editar_Se_Ele_for_Valido()
        {
            var medico = new Medico("medico", "12345-SC");

            var result = servicoMedico.Editar(medico);

            result.Result.Should().BeSuccess();

            repositorioMoq.Verify(x=>x.Editar(medico), Times.Once);
        }

        [TestMethod]
        public void Nao_Deve_Editar_Se_CRM_for_Invalido()
        {
            var medico = new Medico("medico", "5-SC");

            var result = servicoMedico.Editar(medico);

            result.Result.Should().BeFailure();

            repositorioMoq.Verify(x => x.Editar(medico), Times.Never);
        }

        [TestMethod]
        public void Nao_Deve_Editar_Se_Nome_for_Invalido()
        {
            var medico = new Medico("m", "12345-SC");

            var result = servicoMedico.Editar(medico);

            result.Result.Should().BeFailure();

            repositorioMoq.Verify(x => x.Editar(medico), Times.Never);
        }


        [TestMethod]
        public void Deve_Retornar_Fail_Se_Medico_nao_for_Localizado_Por_CRM()
        {
            var medico =  servicoMedico.SelecionarPorCRM("123", Guid.NewGuid());

            medico.Result.Should().BeFailure();

            medico.Result.Reasons[0].Message.Should().Be("Médico não encontrado");

        }

        [TestMethod]
        public void Deve_Retornar_Fail_Se_Medico_nao_for_Localizado_Por_Id()
        {
            var medico = servicoMedico.SelecionarPorId(Guid.NewGuid());

            medico.Result.Should().BeFailure();

            medico.Result.Reasons[0].Message.Should().Be("Médico não encontrado");
        }
    }
}