namespace EAgendaMedica.WebApi.ViewModels.Compartilhado
{
    public class ListarAtividadeViewModel
    {
        public Guid Id { get; set; }
        public string? DataInicio { get; set; }
        public string? HoraInicio { get; set; }
        public string? HoraTermino { get; set; }
    }
}
