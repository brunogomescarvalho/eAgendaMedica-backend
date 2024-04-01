using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.Dominio.ModuloMedico;
using FluentAssertions;

namespace EAgendaUnitTests
{
    [TestClass]
    public class CirurgiaUnitTests
    {
        Medico medico1;
        Medico medico2;
        Medico medico3;


        public CirurgiaUnitTests()
        {
            medico1 = new Medico("Medico1", "12345-SC");
            medico2 = new Medico("Medico2", "12346-SC");
            medico3 = new Medico("Medico3", "12347-SC");
        }

        [TestMethod]
        public void Ao_Gerar_Cirurgia_Entao_Deve_Incluir_Na_Lista_Do_Medico()
        {
            var medicos = new List<Medico>() { medico1 };

            var cirurgia = new Cirurgia(DateTime.Now, TimeSpan.Parse("10:00"), 120, medicos);

            medico1.Cirurgias.Should().Contain(cirurgia);
        }

        [TestMethod]
        public void Ao_Gerar_Cirurgia_Devera_Calcular_A_Hora_Termino()
        {
            var medicos = new List<Medico>() { medico1 };

            var cirurgia = new Cirurgia(new DateTime(2023, 11, 15), TimeSpan.Parse("10:00"), 120, medicos);

            cirurgia.HoraTermino.Should().Be(TimeSpan.Parse("12:00"));
        }

        [TestMethod]
        public void Ao_Gerar_Cirurgia_Devera_Calcular_A_Data_Termino()
        {
            var medicos = new List<Medico>() { medico1 };

            var cirurgia = new Cirurgia(new DateTime(2023, 11, 15), TimeSpan.Parse("23:00"), 120, medicos);

            cirurgia.DataTermino.Should().Be(new DateTime(2023, 11, 16, 01, 00, 00));

        }

        [TestMethod]
        public void Ao_Gerar_Cirurgia_Nao_Devera_Incluir_Medico_Repetido()
        {
            var medicos = new List<Medico>()
            {
                medico1,
                medico1,
                medico1
            };

            var cirurgia = new Cirurgia(new DateTime(2023, 11, 15), TimeSpan.Parse("23:00"), 120, medicos);

            cirurgia.Medicos.Should().HaveCount(1);

        }


        [TestMethod]
        public void Ao_Gerar_Cirurgia_Nao_Devera_Incluir_Medico_Null()
        {

            Medico medicoNull = null!;

            var medicos = new List<Medico>()
            {
                medicoNull!,
                medico1,
                medico2
            };

            var cirurgia = new Cirurgia(new DateTime(2023, 11, 15), TimeSpan.Parse("23:00"), 120, medicos);

            cirurgia.Medicos.Should().HaveCount(2);

        }
    }
}
