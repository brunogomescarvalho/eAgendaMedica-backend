namespace EAgendaMedica.Dominio.Compartilhado
{
    public interface IRepositorioAtividadeBase<T>: IRepositorio<T> where T : Atividade
    {
        Task<List<T>> SelecionarProximos30Dias(Guid usuarioId);

        Task<List<T>> SelecionarUltimos30Dias(Guid usuarioId);

        Task<List<T>> SelecionarParaHoje(Guid usuarioId);

        Task<List<T>> SelecionarPorPeriodo(DateTime dataInicial, DateTime dataFinal, Guid usuarioId);

    }
}
