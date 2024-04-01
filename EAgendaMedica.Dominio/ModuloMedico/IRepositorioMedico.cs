using EAgendaMedica.Dominio.ModuloMedico;

namespace EAgendaMedica.Dominio
{
    public interface IRepositorioMedico : IRepositorio<Medico>
    {
        Task<Medico> SelecionarPorCRM(string crm, Guid usuarioId);
        Task<List<Medico>> SelecionarPorStatus(bool ativo, Guid usuarioId);
        Task<List<Medico>> SelecionarMedicosComAtendimentosNoPeriodo(DateTime dataInicial, DateTime dataFinal, Guid usuarioId);
        Task<List<Medico>> SelecionarMuitos(List<Guid> medicosId);
        Task<bool> Existe(Medico medico);
    }
}