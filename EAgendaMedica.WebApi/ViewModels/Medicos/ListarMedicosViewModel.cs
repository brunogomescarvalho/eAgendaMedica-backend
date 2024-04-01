namespace EAgendaMedica.WebApi.ViewModels.Medicos
{
    public class ListarMedicosViewModel
    {
        public Guid Id { get; set; }
        public string? NomeEPrefixo { get; set; }
        public string? CRM { get; set; }
        public string? Situacao { get; set; }
    }
}
