using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.Dominio.ModuloMedico;
using FluentAssertions;

namespace EAgendaUnitTests
{
    [TestClass]
    public class ConsultasUnitTests
    {
        Medico medico;

        public ConsultasUnitTests()
        {
            medico = new Medico("Medico1","12345-SC");
        }


        [TestMethod]
        public void Ao_Gerar_Consulta_Entao_Deve_Incluir_Na_Lista_Do_Medico()
        {       
            var consulta = new Consulta(DateTime.Now, TimeSpan.Parse("10:00"), 120, medico);

            medico.Consultas.Should().Contain(consulta);
        }

        [TestMethod]
        public void Ao_Gerar_Consulta_Devera_Calcular_A_Hora_Termino()
        {
            var consulta = new Consulta(new DateTime(2023, 11, 15), TimeSpan.Parse("10:00"), 120, medico);

            consulta.HoraTermino.Should().Be(TimeSpan.Parse("12:00"));
        }

        [TestMethod]
        public void Ao_Gerar_Consulta_Devera_Calcular_A_Data_Termino()
        {           
            var consulta = new Consulta(new DateTime(2023, 11, 15), TimeSpan.Parse("23:00"), 120, medico);

            consulta.DataTermino.Should().Be(new DateTime(2023, 11, 16, 01, 00, 00));
        }

    }
}
