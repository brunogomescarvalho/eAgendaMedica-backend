using EAgendaMedica.Dominio;
using EAgendaMedica.Dominio.ModuloAutenticacao;
using EAgendaMedica.Dominio.ModuloMedico;
using EAgendaMedica.Infra.Compartilhado;

namespace EAgendaMedica.Infra.ModuloMedico
{
    public class RepositorioMedico : IRepositorioMedico
    {
        protected DbSet<Medico> registros;

        private readonly EAgendaMedicaDBContext dbContext;

        public RepositorioMedico(IContextoPersistencia contextoPersistencia)
        {
            dbContext = (EAgendaMedicaDBContext)contextoPersistencia;

            registros = dbContext.Set<Medico>();
        }


        public async Task Inserir(Medico registro)
        {
            await registros.AddAsync(registro);
        }

        public async Task<List<Medico>> SelecionarTodos(Guid usuarioId)
        {
            return await registros
                .Where(x => x.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<List<Medico>> SelecionarPorStatus(bool ativo, Guid usuarioId)
        {
            return await registros.Where(x => x.Ativo == ativo)
               .Where(x => x.UsuarioId == usuarioId)
               .ToListAsync();
        }

        public async Task<Medico> SelecionarPorId(Guid id)
        {
            var medico = await registros.Where(x => x.Id == id)
                .Include(x => x.Cirurgias)
                .Include(x => x.Consultas)
                .FirstOrDefaultAsync();

            return medico!;
        }

        public async Task<Medico> SelecionarPorCRM(string crm, Guid usuarioId)
        {
            var medico = await registros.Where(x => x.CRM == crm && x.UsuarioId == usuarioId)
                 .Include(x => x.Consultas)
                 .Include(x => x.Cirurgias)
                 .FirstOrDefaultAsync();

            return medico!;
        }

        public void Excluir(Medico registro)
        {
            registros.Remove(registro);
        }

        public void Editar(Medico registro)
        {
            registros.Update(registro);
        }

        public async Task<List<Medico>> SelecionarMedicosComAtendimentosNoPeriodo(DateTime dataInicial, DateTime dataFinal, Guid usuarioId)
        {
            var medicos = await registros
           .Where(x => x.UsuarioId == usuarioId)
           .Include(x => x.Cirurgias)
           .Include(x => x.Consultas).ToListAsync();

            var medicosComAtendimentos = medicos.Where(medico => medico.TodasAtividades()
            .Any(atividade => atividade.DataInicio.ToUniversalTime() >= dataInicial.ToUniversalTime() && atividade.DataTermino.ToUniversalTime() <= dataFinal.ToUniversalTime())).ToList();

            return medicosComAtendimentos;
        }

        public Task<List<Medico>> SelecionarMuitos(List<Guid> medicosId)
        {
            return registros.Where(x => medicosId.Contains(x.Id))
                .Include(x => x.Consultas)
                .Include(x => x.Cirurgias)
                .ToListAsync();
        }

        public Task<bool> Existe(Medico medico)
        {
            return registros.AnyAsync(x => x.CRM == medico.CRM && x.UsuarioId == medico.UsuarioId && x.Equals(medico) == false);
        }
    }
}
