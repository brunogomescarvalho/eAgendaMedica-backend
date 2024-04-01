using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EAgendaMedica.WebApi.ViewModels.Consultas
{
    public class FormConsultaViewModel
    {
        [Required(ErrorMessage = "Por favor, forneça uma data inicial.")]
        [Display(Name = "Data Inicial")]
        public DateTime DataInicio{ get; set; }

        [Required(ErrorMessage = "Por favor, forneça uma hora inicial.")]
        [Display(Name = "Hora Inicial")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "Por favor, forneça a duração em minutos.")]
        [Display(Name = "Tempo de Duração")]
        public int DuracaoEmMinutos { get; set; }

        [Required(ErrorMessage = "Por Favor, forneça o id do médico.")]
        [Display(Name = "Médico Id")]
        public Guid MedicoId { get; set; }
    }
}
