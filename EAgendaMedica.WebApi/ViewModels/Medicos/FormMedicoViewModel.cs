using EAgendaMedica.Dominio.ModuloMedico;
using System.ComponentModel.DataAnnotations;

namespace EAgendaMedica.WebApi.ViewModels.Medicos
{
    public class FormMedicoViewModel
    {
        [Required(ErrorMessage ="Por favor, forneça o nome do médico.")]
        [Display(Name = "Nome do Médico")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Por favor, forneça o CRM do médico.")]
        [Display(Name = "CRM do Médico")]
        public string? CRM { get; set; }

        public PrefixoEnum Prefixo { get; set; }

    }
}
