using EAgendaMedica.Aplicacao.ModuloConsulta;
using EAgendaMedica.Dominio.Copartilhado;
using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.Dominio.ModuloMedico;
using FluentAssertions;
using Moq;

namespace EAgendaMedica.AplicacaoUnitTests
{
    [TestClass]
    public class ServicoConsultaTests
    {

        Mock<IRepositorioConsulta> repositorioMoq;

        ServicoConsulta servicoConsulta;

        Mock<IContextoPersistencia> contexto;

        public ServicoConsultaTests()
        {
            repositorioMoq = new Mock<IRepositorioConsulta>();

            contexto = new Mock<IContextoPersistencia>();

            servicoConsulta = new ServicoConsulta(repositorioMoq.Object, contexto.Object);
        }


        [TestMethod]
        public async Task NaoDeveCadastrarPossuindoConflitosDeHorarios()
        {
            var medico = new Medico("medico", "12345-sc");

            _ = new Consulta(DateTime.Now, TimeSpan.Parse("10:00"), 120, medico);

            var consulta = new Consulta(DateTime.Now, TimeSpan.Parse("10:00"), 120, medico);

            var result = await servicoConsulta.Inserir(consulta);

            result.IsSuccess.Should().Be(false);

            repositorioMoq.Verify(x => x.Inserir(consulta), Times.Never);
        }


        [TestMethod]
        public async Task NaoDeveEditarPossuindoConflitosDeHorarios()
        {
            var medico = new Medico("medico", "12345-sc");

            _ = new Consulta(DateTime.Now, TimeSpan.Parse("10:00"), 120, medico);

            var consulta = new Consulta(DateTime.Now, TimeSpan.Parse("10:00"), 120, medico);

            var result = await servicoConsulta.Editar(consulta);

            result.IsSuccess.Should().Be(false);

            repositorioMoq.Verify(x => x.Inserir(consulta), Times.Never);

        }

        [TestMethod]
        public async Task NaoDeveCadastrarComDescansoMedicoInsuficiente()
        {
            var medico = new Medico("medico", "12345-sc");

            _ = new Consulta(DateTime.Now, TimeSpan.Parse("10:00"), 120, medico);

            var consulta = new Consulta(DateTime.Now, TimeSpan.Parse("11:05"), 120, medico);

            var result = await servicoConsulta.Inserir(consulta);

            result.IsSuccess.Should().Be(false);

            repositorioMoq.Verify(x => x.Inserir(consulta), Times.Never);

            var msg = "A consulta não pode ser agendada no horário solicitado, pois conflita com outros procedimentos do médico.";

            result.Reasons.Select(x => x.Message).FirstOrDefault().Should().Be(msg);

        }

        [TestMethod]
        public async Task DeveMostrarMensagemAoDescansoserInsuficiente()
        {
            var medico = new Medico("medico", "12345-sc");

            _ = new Consulta(DateTime.Now, TimeSpan.Parse("10:00"), 120, medico);

            var consulta = new Consulta(DateTime.Now, TimeSpan.Parse("12:05"), 120, medico);

            var result = await servicoConsulta.Inserir(consulta);

            var msg = "O médico da consulta não possui descanso suficiente de 20 minutos";

            result.Reasons.Select(x => x.Message).FirstOrDefault().Should().Be(msg);
        }

        [TestMethod]
        public async Task NaoDeveCadastrarComDataInvalida()
        {
            var medico = new Medico("medico", "12345-sc");

            _ = new Consulta(DateTime.Now, TimeSpan.Parse("10:00"), 120, medico);

            var consulta = new Consulta(default, TimeSpan.Parse("15:00"), 120, medico);

            var result = await servicoConsulta.Inserir(consulta);

            result.IsSuccess.Should().Be(false);

            repositorioMoq.Verify(x => x.Inserir(consulta), Times.Never);

            var msg = "Data Inválida";

            result.Reasons.Select(x => x.Message).FirstOrDefault().Should().Be(msg);

        }


        [TestMethod]
        public async Task DeveInformarQuandoConsultaNaoForEncontrada()
        {
            Consulta consulta = null!;

            repositorioMoq.Setup(i => i.SelecionarPorId(Guid.NewGuid()))
                .ReturnsAsync(consulta);

            var result = await servicoConsulta.SelecionarPorId(Guid.NewGuid());

            result.IsSuccess.Should().Be(false);

            var msg = result.Reasons.Select(x => x.Message).FirstOrDefault();

            msg.Should().Be("Consulta não encontrada");

        }
    }
}
