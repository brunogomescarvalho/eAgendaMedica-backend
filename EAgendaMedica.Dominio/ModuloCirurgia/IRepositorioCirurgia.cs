namespace EAgendaMedica.Dominio.ModuloCirurgia
{
    public interface IRepositorioCirurgia : IRepositorioAtividadeBase<Cirurgia>
    {
        Task<List<Cirurgia>> ObterCirurgiasPorMedico(string CRM, Guid usuarioId);
    }
}
