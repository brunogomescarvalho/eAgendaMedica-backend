using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.Infra.Compartilhado;

namespace EAgendaMedica.Infra.ModuloCirurgia
{
    public class RepositorioCirurgia : RepositorioAtividadeBase<Cirurgia>, IRepositorioCirurgia
    {
        public RepositorioCirurgia(IContextoPersistencia contextoPersistencia) : base(contextoPersistencia)
        {
        }

        public async Task<List<Cirurgia>> ObterCirurgiasPorMedico(string CRM, Guid usuarioId)
        {
            return await registros
                .Where(x => x.Medicos
                .Any(x => x.CRM == CRM && x.UsuarioId == usuarioId))
                .Include(x => x.Medicos)
                .ToListAsync();
        }

        public override async Task<Cirurgia> SelecionarPorId(Guid id)
        {
            var registro = await registros.Where(x => x.Id.Equals(id)).Include(x => x.Medicos).FirstOrDefaultAsync();

            return registro!;
        }
    }
}


