namespace EAgendaMedica.WebApi.ViewModels.Medicos
{
    public class VisualizarMedicoViewModel
    {
        public Guid Id { get; set; }
        public string? NomeEPrefixo { get; set; }

        public string? CRM { get; set; }

        public int HorasTotaisTrabalhadas { get; set; }

        public string Situacao { get; set; }

        public List<ListarAtividadesMedicoViewModel>? Atividades { get; set; }
    }
}
