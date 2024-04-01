using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.Infra.Compartilhado;

namespace EAgendaMedica.Infra.ModuloConsulta
{
    public class RepositorioConsulta : RepositorioAtividadeBase<Consulta>, IRepositorioConsulta
    {
        public RepositorioConsulta(IContextoPersistencia contextoPersistencia) : base(contextoPersistencia)
        {

        }

        public async Task<List<Consulta>> ObterConsultasPorMedico(string CRM, Guid usuarioId)
        {
            return await registros.Where(x => x.Medico.CRM == CRM && x.UsuarioId == usuarioId)
                .Include(x => x.Medico)
                .ToListAsync();
        }


        public override async Task<Consulta> SelecionarPorId(Guid id)
        {
            var registro = await registros.Where(x => x.Id.Equals(id)).Include(x => x.Medico).FirstOrDefaultAsync();

            return registro!;
        }


    }
}
