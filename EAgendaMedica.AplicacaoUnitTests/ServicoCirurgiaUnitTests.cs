using EAgendaMedica.Dominio.Copartilhado;
using Moq;
using EAgendaMedica.Aplicacao.ModuloCirurgia;
using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.Dominio.ModuloMedico;
using FluentAssertions;

namespace EAgendaMedica.AplicacaoUnitTests
{
    [TestClass]
    public class ServicoCirurgiaUnitTests
    {
        Mock<IRepositorioCirurgia> repositorioMoq;

        ServicoCirurgia servicoCirurgia;

        Mock<IContextoPersistencia> contexto;

        public ServicoCirurgiaUnitTests()
        {
            repositorioMoq = new Mock<IRepositorioCirurgia>();

            contexto = new Mock<IContextoPersistencia>();

            servicoCirurgia = new ServicoCirurgia(repositorioMoq.Object, contexto.Object);
        }


        [TestMethod]
        public async Task NaoDeveCadastrarPossuindoConflitosDeHorarios()
        {
            var medico1 = new Medico("medico1", "12345-sc");
            var medico2 = new Medico("medico2", "12345-sc");
            var medico3 = new Medico("medico3", "12345-sc");

            _ = new Cirurgia(DateTime.Now, TimeSpan.Parse("10:00"), 120, new List<Medico>() { medico1 });

            var cirurgia = new Cirurgia(DateTime.Now, TimeSpan.Parse("10:00"), 120, new List<Medico>() { medico1, medico2, medico3 });

            var result = await servicoCirurgia.Inserir(cirurgia);

            result.IsSuccess.Should().Be(false);

            repositorioMoq.Verify(x => x.Inserir(cirurgia), Times.Never);

            result.Reasons.Select(x => x.Message)
               .FirstOrDefault().Should()
               .Be("A cirurgia não pode ser agendada no horário solicitado, pois conflita com outros procedimentos de um ou mais médicos.");
        }


        [TestMethod]
        public async Task NaoDeveEditarPossuindoConflitosDeHorarios()
        {
            var medico1 = new Medico("medico1", "12345-sc");
            var medico2 = new Medico("medico2", "12345-sc");
            var medico3 = new Medico("medico3", "12345-sc");

            _ = new Cirurgia(DateTime.Now, TimeSpan.Parse("10:00"), 120, new List<Medico>() { medico1 });

            var cirurgia = new Cirurgia(DateTime.Now, TimeSpan.Parse("10:00"), 120, new List<Medico>() { medico1, medico2, medico3 });

            var result = await servicoCirurgia.Editar(cirurgia);

            result.IsSuccess.Should().Be(false);

            repositorioMoq.Verify(x => x.Inserir(cirurgia), Times.Never);

            result.Reasons.Select(x => x.Message)
                .FirstOrDefault().Should()
                .Be("A cirurgia não pode ser agendada no horário solicitado, pois conflita com outros procedimentos de um ou mais médicos.");

        }

        [TestMethod]
        public async Task DeveMostrarMensagemAoDescansoserInsuficiente()
        {
            var medico = new Medico("medico", "12345-sc");

            _ = new Cirurgia(DateTime.Now, TimeSpan.Parse("10:00"), 120, new List<Medico>() { medico });

            var cirurgia = new Cirurgia(DateTime.Now, TimeSpan.Parse("15:55"), 120, new List<Medico>() { medico });

            var result = await servicoCirurgia.Inserir(cirurgia);

            var msg = "A cirurgia possui um ou mais médicos com descanso inferior a 240 minutos";

            result.Reasons.Select(x => x.Message).FirstOrDefault().Should().Be(msg);
        }

        [TestMethod]
        public async Task NaoDeveCadastrarComDescansoMedicoInsuficiente()
        {
            var medico = new Medico("medico1", "12345-sc");

            _ = new Cirurgia(DateTime.Now, TimeSpan.Parse("10:00"), 120, new List<Medico>() { medico });

            var cirurgia = new Cirurgia(DateTime.Now, TimeSpan.Parse("15:00"), 120, new List<Medico>() { medico });

            var result = await servicoCirurgia.Inserir(cirurgia);

            result.IsSuccess.Should().Be(false);

            repositorioMoq.Verify(x => x.Inserir(cirurgia), Times.Never);

        }

        [TestMethod]
        public async Task NaoDeveCadastrarComDataInvalida()
        {
            var medico = new Medico("medico1", "12345-sc");

            _ = new Cirurgia(DateTime.Now, TimeSpan.Parse("10:00"), 120, new List<Medico>() { medico });

            var cirurgia = new Cirurgia(default, TimeSpan.Parse("15:00"), 120, new List<Medico>() { medico });

            var result = await servicoCirurgia.Inserir(cirurgia);

            result.IsSuccess.Should().Be(false);

            repositorioMoq.Verify(x => x.Inserir(cirurgia), Times.Never);

            var msg = "Data Inválida";

            result.Reasons.Select(x => x.Message).FirstOrDefault().Should().Be(msg);


        }



        [TestMethod]
        public async Task DeveInformarQuandoCirurgiaNaoForEncontrada()
        {
            Cirurgia cirurgia = null!;

            repositorioMoq.Setup(i => i.SelecionarPorId(Guid.NewGuid()))
                .ReturnsAsync(cirurgia);

            var result = await servicoCirurgia.SelecionarPorId(Guid.NewGuid());

            result.IsSuccess.Should().Be(false);

            var msg = result.Reasons.Select(x => x.Message).FirstOrDefault();

            msg.Should().Be("Cirurgia não encontrada");

        }
    }
}
