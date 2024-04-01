using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.Dominio.ModuloMedico;
using FluentAssertions;

namespace EAgendaUnitTests
{
    [TestClass]
    public class VerificadorDescansoUnitTests
    {
        private readonly DateTime hoje = DateTime.Now;

        private readonly TimeSpan dozeHoras = TimeSpan.Parse("12:00");
        private readonly TimeSpan quatorzeHoras = TimeSpan.Parse("14:00");
        private readonly TimeSpan vinteHoras = TimeSpan.Parse("20:00");

        private readonly Medico medico = new Medico("Medico1", "12345-SC");


        [TestMethod]
        public void VerificaDescancoAposConsulta_RegistroAnterior()
        {
            medico.AdicionarConsulta(new Consulta(hoje, TimeSpan.Parse("11:00"), 60, medico));

            var consulta = new Consulta(hoje, dozeHoras, 60, medico);

            consulta.VerificarDescansoMedico().Should().BeFalse();
        }

        [TestMethod]
        public void VerificaDescancoAposConsulta_ResgistroPosterior()
        {
            medico.AdicionarConsulta(new Consulta(hoje, TimeSpan.Parse("11:00"), 60, medico));

            var consulta = new Consulta(hoje, TimeSpan.Parse("09:55"), 60, medico);

            consulta.VerificarDescansoMedico().Should().BeFalse();
        }

       
        [TestMethod]
        public void VerificaDescansoAposCirurgia_ResgistroAnterior()
        {
            var medicos = new List<Medico>() { medico };

            medico.AdicionarCirurgia(new Cirurgia(hoje, quatorzeHoras, 120, medicos));

            var cirurgia = new Cirurgia(hoje, vinteHoras, 120, medicos);

            cirurgia.VerificarDescansoMedico().Should().BeFalse();
        }

        [TestMethod]
        public void VerificaDescansoAposCirurgia_ResgistroPosterior()
        {
            var medicos = new List<Medico>() { medico };

            medico.AdicionarCirurgia(new Cirurgia(hoje, vinteHoras, 120, medicos));

            var cirurgia = new Cirurgia(hoje, TimeSpan.Parse("17:00"), 120, medicos);

            cirurgia.VerificarDescansoMedico().Should().BeFalse();
        }
    }
}