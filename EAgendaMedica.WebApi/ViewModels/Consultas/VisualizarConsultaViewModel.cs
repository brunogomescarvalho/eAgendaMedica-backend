using EAgendaMedica.WebApi.ViewModels.Medicos;

namespace EAgendaMedica.WebApi.ViewModels.Consultas
{
    public class VisualizarConsultaViewModel
    {     
        public string? DataInicio { get; set; }
        public string? DataTermino { get; set; }
        public string? HoraInicio { get; set; }
        public string? HoraTermino { get; set; }
        public ListarMedicosViewModel? Medico { get; set; }
    }

    public class VisualizarConsultaResponseViewModel
    {
        public string? DataInicio { get; set; }
        public string? DataTermino { get; set; }
        public string? HoraInicio { get; set; }
        public string? HoraTermino { get; set; }
        public FormMedicoViewModel? Medico { get; set; }

    }
}
