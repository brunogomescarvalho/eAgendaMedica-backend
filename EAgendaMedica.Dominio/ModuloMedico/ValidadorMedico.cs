using FluentValidation.Results;
using System.Text.RegularExpressions;

namespace EAgendaMedica.Dominio.ModuloMedico
{
    public class ValidadorMedico : AbstractValidator<Medico>
    {
        public ValidadorMedico()
        {
            RuleFor(x => x.Nome).MinimumLength(3)
                .NotNull()
                .WithMessage("NOME é campo obrigatório com no mínimo 3 letras");

            RuleFor(x => x.CRM).Custom(ValidarCRM);
          
        }

        private void ValidarCRM(string crm, ValidationContext<Medico> context)
        {
            if(string.IsNullOrEmpty(crm))
            {
                context.AddFailure("CRM é campo obrigatório");

                return;
            }


            string pattern = @"^\d{5}-[A-Z]{2}$";

            if (!Regex.IsMatch(crm.ToUpper(), pattern))
            {
                context.AddFailure("CRM inválido. O CRM deve estar no formato 12345-XX");
            }
        }
    }
}
