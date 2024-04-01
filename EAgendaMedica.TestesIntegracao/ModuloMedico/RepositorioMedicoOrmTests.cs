using EAgendaMedica.Dominio.Compartilhado;
using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.Dominio.ModuloMedico;
using EAgendaMedica.TestesIntegracao.Compartilhado;
using FluentAssertions;

namespace EAgendaMedica.TestesIntegracao.ModuloMedico
{
    [TestClass]
    public class RepositorioMedicoOrmTests : TestesIntegracaoBase
    {
        [TestMethod]
        public async Task Deve_Selecionar_Medicos_Com_Atendimento_No_Periodo()
        {
            //arrange
            var medico0 = new Medico("medico0", "12345-SC");
            var medico1 = new Medico("medico1", "22345-SC");
            var medico2 = new Medico("medico2", "32345-SC");
            var medico3 = new Medico("medico3", "42345-SC");
            var medico4 = new Medico("medico4", "52345-SC");

            var medicos = new List<Medico>() { medico0, medico1, medico2, medico3, medico4 };

            medicos.ForEach(me => repositorioMedico.Inserir(me));

            var atividades = new List<Atividade>()
            {
                new Consulta(DateTime.Now.AddMonths(1), TimeSpan.Parse("10:00"), 150, medico0),
                new Consulta(DateTime.Now.AddMonths(1), TimeSpan.Parse("10:00"), 150, medico0),
                new Consulta(DateTime.Now.AddMonths(1), TimeSpan.Parse("10:00"), 150, medico0),
                new Consulta(DateTime.Now.AddMonths(1), TimeSpan.Parse("10:00"), 150, medico0),
                new Consulta(DateTime.Now.AddMonths(1), TimeSpan.Parse("10:00"), 150, medico0), //fora do range

                new Consulta(DateTime.Now, TimeSpan.Parse("12:00"), 170, medico1),
                new Consulta(DateTime.Now, TimeSpan.Parse("14:01"), 160, medico2),
                new Consulta(DateTime.Now, TimeSpan.Parse("15:02"), 150, medico3),
                new Consulta(DateTime.Now, TimeSpan.Parse("15:02"), 150, medico4),

                new Cirurgia(DateTime.Now.AddDays(1), TimeSpan.Parse("10:00"), 100, medicos.GetRange(1, 1)), //medico 1
                new Cirurgia(DateTime.Now.AddDays(1), TimeSpan.Parse("12:00"), 100, medicos.GetRange(2, 1)),
                new Cirurgia(DateTime.Now.AddDays(1), TimeSpan.Parse("14:01"), 100, medicos.GetRange(3, 1)),
                new Cirurgia(DateTime.Now.AddDays(1), TimeSpan.Parse("15:02"), 100, medicos.GetRange(4, 1)),
                new Cirurgia(DateTime.Now.AddDays(1), TimeSpan.Parse("16:03"), 100, medicos.GetRange(1, 1)),

            };

            foreach (var item in atividades)
            {
                if (item is Cirurgia cirurgia)
                    await repositorioCirurgia.Inserir(cirurgia);

                else
                    await repositorioConsulta.Inserir((Consulta)item);

            }
            await dbContext.SaveChangesAsync();

            //action
            var medicosQueAtenderam = await repositorioMedico.SelecionarMedicosComAtendimentosNoPeriodo(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(2), new Guid());

            //assert
            medicosQueAtenderam.Count.Should().Be(4); //o método traz somente médicos que possuem algum atendimento, médico 4 , não possui...
        }

        [TestMethod]
        public async Task Deve_Cadastrar_Novo_Medico()
        {
            //arrange
            var medico = new Medico("medico", "12345-SC");

            await repositorioMedico.Inserir(medico);

            await dbContext.SaveChangesAsync();
            //action
            var medicoBuscados = await repositorioMedico.SelecionarTodos(new Guid());
            //assert
            medicoBuscados[0].Should().BeSameAs(medico);
        }

        [TestMethod]
        public async Task Deve_Selecionar_Medico_Por_Crm()
        {
            //arrange
            var medico = new Medico("medico", "12345-SC");

            await repositorioMedico.Inserir(medico);

            await dbContext.SaveChangesAsync();
            //action
            var medicoBuscado = await repositorioMedico.SelecionarPorCRM("12345-SC", Guid.NewGuid());
            //assert
            medicoBuscado.Should().BeSameAs(medico);
        }

        [TestMethod]
        public async Task Deve_Selecionar_Medico_Por_Id()
        {
            //arrange
            var medico = new Medico("medico", "12345-SC");

            var id = medico.Id;

            await repositorioMedico.Inserir(medico);

            await dbContext.SaveChangesAsync();
            //action
            var medicoBuscado = await repositorioMedico.SelecionarPorId(id);
            //assert
            medicoBuscado.Should().BeSameAs(medico);
        }


        [TestMethod]
        public async Task Deve_Editar_Medico()
        {
            //arrange
            var medico = new Medico("medico", "12345-SC");

            var id = medico.Id;

            await repositorioMedico.Inserir(medico);

            await dbContext.SaveChangesAsync();

            //action
            var medicoBuscado = await repositorioMedico.SelecionarPorId(id);

            medicoBuscado.Nome = "Nome Editado";

            repositorioMedico.Editar(medicoBuscado);

            dbContext.SaveChanges();

            //assert
            var medicoEditado = await repositorioMedico.SelecionarPorId(id);

            medicoEditado.Nome.Should().Be("Nome Editado");
        }

        [TestMethod]
        public async Task Deve_Deletar_Medico()
        {
            //arrange
            var medico = new Medico("medico", "12345-SC");

            await repositorioMedico.Inserir(medico);

            await dbContext.SaveChangesAsync();
            //action
            repositorioMedico.Excluir(medico);

            await dbContext.SaveChangesAsync();
            //assert
            var medicos = await repositorioMedico.SelecionarTodos(new Guid());

            medicos.Count.Should().Be(0);


        }
    }
}
