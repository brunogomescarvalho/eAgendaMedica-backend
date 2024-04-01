namespace EAgendaMedica.WebApi.ViewModels.Medicos
{
    public class ListarRankingMedicosViewModel
    {
        public Guid Id { get; set; }
        public string? NomeEPrefixo { get; set; }
        public string? CRM { get; set; }
        public int HorasTrabalhadasNoPeriodo { get; set; }
    }
}
