using EAgendaMedica.Dominio.Compartilhado;

namespace EAgendaMedica.Infra.Compartilhado
{
    public class RepositorioAtividadeBase<T> : IRepositorioAtividadeBase<T> where T : Atividade
    {
        protected DbSet<T> registros;

        private readonly EAgendaMedicaDBContext dbContext;

        public RepositorioAtividadeBase(IContextoPersistencia contextoPersistencia)
        {
            dbContext = (EAgendaMedicaDBContext)contextoPersistencia;

            registros = dbContext.Set<T>();
        }

        public virtual async Task Inserir(T registro)
        {
            await registros.AddAsync(registro);
        }

        public virtual async Task<List<T>> SelecionarTodos(Guid usuarioId)
        {
            return await registros.Where(x => x.UsuarioId == usuarioId).ToListAsync();
        }

        public virtual async Task<T> SelecionarPorId(Guid id)
        {
            var registro = await registros.Where(x => x.Id == id).FirstOrDefaultAsync();

            return registro!;
        }

        public virtual void Excluir(T registro)
        {
            registros.Remove(registro);
        }

        public virtual void Editar(T registro)
        {
            registros.Update(registro);
        }

        public virtual async Task<List<T>> SelecionarParaHoje(Guid usuarioId)
        {
            return await registros
                .Where(x => x.DataInicio.Date.ToUniversalTime() == DateTime.Now.Date.ToUniversalTime())
                .Where(x=>x.UsuarioId == usuarioId).ToListAsync();
        }

        public virtual async Task<List<T>> SelecionarPorPeriodo(DateTime dataInicial, DateTime dataFinal, Guid usuarioId)
        {
            return await registros.Where(x => x.DataInicio.ToUniversalTime() >= dataInicial.ToUniversalTime() 
            && x.DataInicio.ToUniversalTime() <= dataFinal.ToUniversalTime() 
            && x.UsuarioId == usuarioId).ToListAsync();
        }

        public virtual async Task<List<T>> SelecionarProximos30Dias(Guid usuarioId)
        {
            return await registros.Where(x => x.DataInicio.ToUniversalTime() > DateTime.Today.ToUniversalTime()
            && x.DataInicio.ToUniversalTime() < DateTime.Now.Date.AddDays(30).ToUniversalTime() 
            && x.UsuarioId == usuarioId).ToListAsync();
        }

        public virtual async Task<List<T>> SelecionarUltimos30Dias(Guid usuarioId)
        {
            return await registros.Where(x => x.DataInicio.ToUniversalTime() < DateTime.Today.ToUniversalTime() 
            && x.DataInicio.ToUniversalTime() > DateTime.Now.AddDays(-30).ToUniversalTime() 
            && x.UsuarioId == usuarioId).ToListAsync();
        }
    }
}
