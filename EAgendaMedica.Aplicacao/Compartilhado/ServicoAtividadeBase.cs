using FluentResults;
using FluentValidation;
using Serilog;
namespace EAgendaMedica.Aplicacao.Compartilhado
{

    public abstract class ServicoAtividadeBase<TAtividade, TValidador> where TValidador : AbstractValidator<TAtividade>, new()
    {      
        protected virtual Result Validar(TAtividade obj)
        {
            var validador = new TValidador();

            var resultadoValidacao = validador.Validate(obj);

            var erros = new List<Error>();

            foreach (var validationFailure in resultadoValidacao.Errors)
            {
                Log.Logger.Warning(validationFailure.ErrorMessage);

                erros.Add(new Error(validationFailure.ErrorMessage));
            }

            if (erros.Any())
                return Result.Fail(erros);

            return Result.Ok();
        }


        protected Result<List<TAtividade>> Processarlista(List<TAtividade> atividades)
        {
            var tipo = typeof(TAtividade);

            var msg = $"Nenhuma {tipo.Name.ToLower()} foi encontrada para a opção selecionada.";

            if (atividades.Count == 0)
            {
                Log.Logger.Warning(msg);

                return Result.Ok(atividades);
            }

            return Result.Ok(atividades);
        }
    }
}

