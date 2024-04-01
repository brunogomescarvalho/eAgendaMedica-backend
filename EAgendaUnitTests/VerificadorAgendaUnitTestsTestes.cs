using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.Dominio.ModuloMedico;
using FluentAssertions;

namespace EAgendaUnitTests
{
    [TestClass]
    public class VerificadorAgendaUnitTestsTestes
    {
        private readonly DateTime hoje = DateTime.Now;
        private readonly TimeSpan vinteHoras = TimeSpan.Parse("20:00");
        private readonly Medico medico = new Medico("Medico1", "12345-SC");

        [TestMethod]
        public void VerificaConflitoHoraInicial()
        {
            var medicos = new List<Medico>() { medico };

            medico.AdicionarCirurgia(new Cirurgia(hoje, vinteHoras, 60, medicos));

            var cirurgia = new Cirurgia(hoje, vinteHoras, 120, medicos);     

            cirurgia.VerificarAgendaMedico().Should().BeFalse();
         
        }


        [TestMethod]
        public void VerificaAtividadeAcabaDurante()
        {
            _ = new Consulta(hoje, vinteHoras, 120, medico);

            var paraVerificar = new Consulta(hoje, TimeSpan.Parse("19:00"), 120, medico);

            paraVerificar.VerificarAgendaMedico().Should().Be(false);
        }

        [TestMethod]
        public void VerificaAtividadeComecaDurante()
        {
            _ = new Consulta(hoje, vinteHoras, 120, medico);

            var paraVerificar = new Consulta(hoje, TimeSpan.Parse("21:00"), 120, medico);

            paraVerificar.VerificarAgendaMedico().Should().Be(false);
        }

        [TestMethod]
        public void VerificaAtividadeComecaAntesETerminaDepois()
        {
            _ = new Consulta(hoje, vinteHoras, 60, medico);

            var paraVerificar = new Consulta(hoje, TimeSpan.Parse("19:00"), 240, medico);

            paraVerificar.VerificarAgendaMedico().Should().Be(false);
        }

        [TestMethod]
        public void VerificaAtividadeComecaETerminaDurante()
        {
            _ = new Consulta(hoje, vinteHoras, 120, medico);

            var paraVerificar = new Consulta(hoje, TimeSpan.Parse("20:30"), 60, medico);

            paraVerificar.VerificarAgendaMedico().Should().Be(false);
        }
    }
}