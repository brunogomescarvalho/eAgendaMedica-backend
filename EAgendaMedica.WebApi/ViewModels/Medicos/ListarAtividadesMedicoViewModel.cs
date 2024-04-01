using System.ComponentModel;

namespace EAgendaMedica.WebApi.ViewModels.Medicos
{
    public class ListarAtividadesMedicoViewModel
    {
        public Guid Id { get; set; }
        public string? DataInicio { get; set; }
        public string? HoraInicio { get; set; } 
        public string? HoraTermino { get; set; }
        public string? TipoAtividade { get; set; }
    }

}
