using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.Dominio.ModuloMedico;
using EAgendaMedica.TestesIntegracao.Compartilhado;
using FizzWare.NBuilder;
using FluentAssertions;

namespace EAgendaMedica.TestesIntegracao.ModuloConsulta
{
    [TestClass]
    public class RepositorioConsultaOrmTestes : TestesIntegracaoBase
    {

        [TestMethod]
        public async Task Deve_Cadastrar_Nova_Consulta()
        {
            //arrange
            var medico = new Medico("medico", "12345-SC");

            await repositorioMedico.Inserir(medico);

            await dbContext.SaveChangesAsync();

            var medicoParaConsulta = await repositorioMedico.SelecionarPorCRM("12345-SC", Guid.NewGuid());

            var consulta = new Consulta()
            {
                DataInicio = DateTime.Now,
                HoraInicio = TimeSpan.Parse("10:00:00"),
                DuracaoEmMinutos = 120,
                Medico = medicoParaConsulta
            };

            //action
            await repositorioConsulta.Inserir(consulta);

            await dbContext.SaveChangesAsync();

            //assert
            repositorioConsulta.SelecionarTodos(new Guid()).Result.Should().HaveCount(1);

        }

        [TestMethod]
        public async Task Deve_Editar_Consulta()
        {
            //arrange
            var medico = new Medico("medico", "12345-SC");

            await repositorioMedico.Inserir(medico);

            await dbContext.SaveChangesAsync();

            var medicoParaConsulta = await repositorioMedico.SelecionarPorCRM("12345-SC", Guid.NewGuid());

            var consulta = new Consulta()
            {
                DataInicio = DateTime.Now,
                HoraInicio = TimeSpan.Parse("10:00:00"),
                DuracaoEmMinutos = 120,
                Medico = medicoParaConsulta
            };

            await repositorioConsulta.Inserir(consulta);

            await dbContext.SaveChangesAsync();


            //action
            var lista = await repositorioConsulta.SelecionarTodos(new Guid());

            var consultaParaEditar = lista[0];

            consultaParaEditar.DuracaoEmMinutos = 60;

            repositorioConsulta.Editar(consultaParaEditar);

            dbContext.SaveChanges();

            //assert
            var listaNova = await repositorioConsulta.SelecionarTodos(new Guid());

            var consultaEditada = listaNova[0];

            consultaEditada.DuracaoEmMinutos.Should().Be(60);

            consultaEditada.HoraTermino.Should().Be(TimeSpan.Parse("11:00"));
        }

        [TestMethod]
        public async Task Deve_Excluir_Consulta()
        {
            //arrange
            var consulta = Builder<Consulta>.CreateNew().Build();

            consulta.Medico = new Medico("medico", "12345-SC");

            await repositorioConsulta.Inserir(consulta);

            await dbContext.SalvarDados();

            //action
            repositorioConsulta.Excluir(consulta);

            await dbContext.SaveChangesAsync();

            //assert
            var consultas = await repositorioConsulta.SelecionarTodos(new Guid());

            consultas.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task Deve_BuscarConsultas_De_Um_Medico()
        {
            var medico0 = new Medico("medico0", "12345-SC"); // medico escolhido
            var medico1 = new Medico("medico1", "22345-SC");
            var medico2 = new Medico("medico2", "32345-SC");
            var medico3 = new Medico("medico3", "42345-SC");
            var medico4 = new Medico("medico4", "52345-SC");

            var medicos = new List<Medico>() { medico0, medico1, medico2, medico3, medico4 };

            medicos.ForEach(me => repositorioMedico.Inserir(me));

            var consultas = new List<Consulta>()
            {
                new Consulta(DateTime.Now, TimeSpan.Parse("12:00"), 170, medico1),
                new Consulta(DateTime.Now, TimeSpan.Parse("14:01"), 160, medico2),
                new Consulta(DateTime.Now, TimeSpan.Parse("15:02"), 150, medico3),
                new Consulta(DateTime.Now, TimeSpan.Parse("15:02"), 150, medico0) //consulta com o medico
            };

            consultas.ForEach(async (x) => await repositorioConsulta.Inserir(x));

            await dbContext.SalvarDados();

            var atendimento = await repositorioConsulta.ObterConsultasPorMedico("12345-SC", Guid.NewGuid());

            atendimento[0].Should().BeEquivalentTo(consultas[3]);

            atendimento.Count.Should().Be(1);

        }
    }
}
