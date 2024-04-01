using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.Dominio.ModuloMedico;
using FluentAssertions;


namespace EAgendaUnitTests
{
    [TestClass]
    public class MedicoUnitTest
    {

        [TestMethod]
        public void DeveIncluirUmaConsulta()
        {
            var medico = new Medico("Medico", "12345-SC");

            medico.AdicionarConsulta(new Consulta(DateTime.Now, DateTime.Now.TimeOfDay, 60, medico));

            medico.Consultas.Count.Should().Be(1);
        }

        [TestMethod]
        public void Nao_DeveIncluirUm_Atendimento_JaExistente()
        {
            var medico = new Medico("Medico", "12345-SC");

            var consulta = (new Consulta(DateTime.Now, DateTime.Now.TimeOfDay, 60, medico));

            medico.AdicionarConsulta(consulta);

            medico.AdicionarConsulta(consulta);

            medico.Consultas.Count.Should().Be(1);
        }


        [TestMethod]
        public void Deve_Retornar_Atividades_DoDia()
        {
            var medico = new Medico("Medico", "12345-SC");

            medico.AdicionarConsulta(new Consulta(DateTime.Now.AddDays(-1), DateTime.Now.TimeOfDay, 60, medico));
            medico.AdicionarConsulta(new Consulta(DateTime.Now, DateTime.Now.TimeOfDay, 60, medico));
            medico.AdicionarConsulta(new Consulta(DateTime.Now.AddDays(1), DateTime.Now.TimeOfDay, 60, medico));

            var consultas = medico.AtividadesDoDia(DateTime.Now);

            consultas.Count.Should().Be(1);

        }

        [TestMethod]
        public void Deve_Obter_Horas_Totais_Trabalhadas()
        {
            var medico = new Medico("Medico", "12345-SC");

            medico.AdicionarConsulta(new Consulta(DateTime.Now.AddDays(-1), DateTime.Now.TimeOfDay, 60, medico));
            medico.AdicionarConsulta(new Consulta(DateTime.Now, DateTime.Now.TimeOfDay, 60, medico));
            medico.AdicionarConsulta(new Consulta(DateTime.Now.AddDays(1), DateTime.Now.TimeOfDay, 60, medico));

            medico.HorasTotaisTrabalhadas.Should().Be(3);
        }

       
        [TestMethod]
        public void Deve_Calcular_Horas_Totais_Trabalhadas_Por_Periodo()
        {
            var medico = new Medico("Medico", "12345-SC");

            //fora do range
            medico.AdicionarConsulta(new Consulta(DateTime.Now.AddDays(-10), DateTime.Now.TimeOfDay, 60, medico));
            medico.AdicionarConsulta(new Consulta(DateTime.Now.AddDays(-10), DateTime.Now.TimeOfDay, 60, medico));

            medico.AdicionarConsulta(new Consulta(DateTime.Now.AddDays(-1), DateTime.Now.TimeOfDay, 60, medico));
            medico.AdicionarConsulta(new Consulta(DateTime.Now, DateTime.Now.TimeOfDay, 60, medico));
            medico.AdicionarConsulta(new Consulta(DateTime.Now.AddDays(1), DateTime.Now.TimeOfDay, 60, medico));

            //fora do range
            medico.AdicionarConsulta(new Consulta(DateTime.Now.AddDays(10), DateTime.Now.TimeOfDay, 60, medico));
            medico.AdicionarConsulta(new Consulta(DateTime.Now.AddDays(10), DateTime.Now.TimeOfDay, 60, medico));

            var horasTrabalhadasNoPeriodo = medico.ObterHorasTrabalhadasPorPeriodo(DateTime.Now.AddDays(-5), DateTime.Now.AddDays(5));

            horasTrabalhadasNoPeriodo.Should().Be(3);
        }

        [TestMethod]
        public void Deve_Retornar_Todas_As_Consultas_E_Cirurgias_Em_Uma_Unica_Lista()
        {
            var medico = new Medico("Medico", "12345-SC");

            medico.AdicionarConsulta(new Consulta(DateTime.Now.AddDays(-10), DateTime.Now.TimeOfDay, 60, medico));
            medico.AdicionarConsulta(new Consulta(DateTime.Now.AddDays(-10), DateTime.Now.TimeOfDay, 60, medico));

            medico.AdicionarCirurgia(new Cirurgia(DateTime.Now.AddDays(10), DateTime.Now.TimeOfDay, 60, new List<Medico>() { medico }));
            medico.AdicionarCirurgia(new Cirurgia(DateTime.Now.AddDays(10), DateTime.Now.TimeOfDay, 60, new List<Medico>() { medico }));

            medico.TodasAtividades().Count.Should().Be(4);

        }
    }
}
