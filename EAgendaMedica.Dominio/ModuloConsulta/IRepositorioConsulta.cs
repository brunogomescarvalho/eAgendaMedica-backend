using EAgendaMedica.Dominio.ModuloCirurgia;

namespace EAgendaMedica.Dominio.ModuloConsulta
{
    public interface IRepositorioConsulta : IRepositorioAtividadeBase<Consulta>
    {
        Task<List<Consulta>> ObterConsultasPorMedico(string CRM, Guid usuarioId);
    }
}
