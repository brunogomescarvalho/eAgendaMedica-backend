using EAgendaMedica.Dominio.ModuloMedico;

namespace EAgendaMedica.Dominio.ModuloCirurgia
{
    public class ValidadorCirurgia : AbstractValidator<Cirurgia>
    {
        public ValidadorCirurgia()
        {
            RuleFor(x => x.Medicos).NotEmpty().NotNull().WithMessage("Falha oa incluir médicos. Verifique os dados informados");

            RuleFor(x => x.DuracaoEmMinutos).GreaterThan(119).WithMessage("O tempo mínimo para uma cirurgia é de 120 minutos");

            RuleFor(x => x.DataInicio.Date).NotEqual(DateTime.MinValue).WithMessage("Data Inválida");

            RuleFor(x => x.Medicos).Custom(VerificarDisponibilidade);
        }

        private void VerificarDisponibilidade(List<Medico> list, ValidationContext<Cirurgia> context)
        {
            var ehValido = context.InstanceToValidate.VerificarDescansoMedico();

            if (ehValido == false)
            {
                context.AddFailure("A cirurgia possui um ou mais médicos com descanso inferior a 240 minutos");

                return;
            }

            ehValido = context.InstanceToValidate.VerificarAgendaMedico();

            if (ehValido == false)
            {
                context.AddFailure("A cirurgia não pode ser agendada no horário solicitado, pois conflita com outros procedimentos de um ou mais médicos.");
            };

        }
    }
}
