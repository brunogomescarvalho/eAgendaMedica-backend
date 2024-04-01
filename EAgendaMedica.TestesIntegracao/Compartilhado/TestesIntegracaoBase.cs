using EAgendaMedica.Dominio;
using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.Dominio.ModuloMedico;
using EAgendaMedica.Infra.Compartilhado;
using EAgendaMedica.Infra.ModuloCirurgia;
using EAgendaMedica.Infra.ModuloConsulta;
using EAgendaMedica.Infra.ModuloMedico;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EAgendaMedica.TestesIntegracao.Compartilhado
{
    [TestClass]
    public class TestesIntegracaoBase
    {

        protected EAgendaMedicaDBContext dbContext;

        protected IRepositorioConsulta repositorioConsulta;

        protected IRepositorioCirurgia repositorioCirurgia;

        protected IRepositorioMedico repositorioMedico;

        public TestesIntegracaoBase()
        {
            dbContext = ObterContext();

            repositorioConsulta = new RepositorioConsulta(dbContext);

            repositorioMedico = new RepositorioMedico(dbContext);

            repositorioCirurgia = new RepositorioCirurgia(dbContext);

            LimparTabelas(dbContext);

        }

        public EAgendaMedicaDBContext ObterContext()
        {
            string[] args = new string[] { "Testing" };

            var dbContextFactory = new EAgendaMedicaContextFactory();

            var dbContext = dbContextFactory.CreateDbContext(args);

            AtualizarBancoDados(dbContext);

            return dbContext;

        }

        private void AtualizarBancoDados(DbContext db)
        {
            var migracoesPendentes = db.Database.GetPendingMigrations();

            if (migracoesPendentes.Any())
            {
                db.Database.Migrate();            
            }
        }

        private static void LimparTabelas(EAgendaMedicaDBContext dbContext)
        {
            dbContext.Set<Consulta>().RemoveRange(dbContext.Set<Consulta>());
            dbContext.Set<Cirurgia>().RemoveRange(dbContext.Set<Cirurgia>());
            dbContext.Set<Medico>().RemoveRange(dbContext.Set<Medico>());
            dbContext.SaveChanges();
        }
    }
}