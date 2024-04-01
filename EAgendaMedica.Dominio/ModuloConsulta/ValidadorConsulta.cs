using EAgendaMedica.Dominio.ModuloMedico;

namespace EAgendaMedica.Dominio.ModuloConsulta
{
    public class ValidadorConsulta : AbstractValidator<Consulta>
    {
        public ValidadorConsulta()
        {
            RuleFor(x => x.Medico).NotNull();

            RuleFor(x => x.DataInicio.Date).NotEqual(DateTime.MinValue).WithMessage("Data Inválida");

            RuleFor(x => x.DuracaoEmMinutos).LessThan(121).WithMessage("O tempo máximo para uma consulta é de 120 minutos");

            RuleFor(x => x.Medico).Custom(VerificarDisponibilidade);
        }

        private void VerificarDisponibilidade(Medico medico, ValidationContext<Consulta> context)
        {
            var ehValido = context.InstanceToValidate.VerificarDescansoMedico();

            if (ehValido == false)
            {
                context.AddFailure("O médico da consulta não possui descanso suficiente de 20 minutos");

                return;
            }

            ehValido = context.InstanceToValidate.VerificarAgendaMedico();

            if (ehValido == false)
            {
                context.AddFailure("A consulta não pode ser agendada no horário solicitado, pois conflita com outros procedimentos do médico.");
            }
        }
    }
}
