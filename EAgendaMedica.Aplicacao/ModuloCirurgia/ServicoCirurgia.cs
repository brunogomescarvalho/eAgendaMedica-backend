using EAgendaMedica.Aplicacao.Compartilhado;
using EAgendaMedica.Dominio.Copartilhado;
using EAgendaMedica.Dominio.ModuloCirurgia;
using FluentResults;
using Serilog;

namespace EAgendaMedica.Aplicacao.ModuloCirurgia
{
    public class ServicoCirurgia : ServicoAtividadeBase<Cirurgia, ValidadorCirurgia>
    {
        private readonly IRepositorioCirurgia repositorioCirurgia;

        private readonly IContextoPersistencia contextoPersistencia;

        public ServicoCirurgia(IRepositorioCirurgia repositorioCirurgia, IContextoPersistencia contextoPersistencia)
        {
            this.repositorioCirurgia = repositorioCirurgia;
            this.contextoPersistencia = contextoPersistencia;
        }

        public async Task<Result<Cirurgia>> Inserir(Cirurgia cirurgia)
        {
            var resultado = Validar(cirurgia);

            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);

            await repositorioCirurgia.Inserir(cirurgia);

            await contextoPersistencia.SalvarDados();

            return Result.Ok(cirurgia);

        }

        public async Task<Result<Cirurgia>> Editar(Cirurgia cirurgia)
        {
            var resultado = Validar(cirurgia);

            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);

            repositorioCirurgia.Editar(cirurgia);

            await contextoPersistencia.SalvarDados();

            return Result.Ok(cirurgia);

        }

        public async Task<Result> Excluir(Cirurgia cirurgia)
        {
            repositorioCirurgia.Excluir(cirurgia);

            await contextoPersistencia.SalvarDados();

            return Result.Ok();
        }

        public async Task<Result<Cirurgia>>SelecionarPorId(Guid id)
        {
            var cirurgia = await repositorioCirurgia.SelecionarPorId(id);

            if (cirurgia == null)
            {
                Log.Logger.Warning("Cirurgia {CirurgiaId} não encontrada", id);

                return Result.Fail("Cirurgia não encontrada");
            }

            return Result.Ok(cirurgia);
        }

        public async Task<Result<List<Cirurgia>>> SelecionarTodos(Guid usuarioId)
        {
            var cirurgias = await repositorioCirurgia.SelecionarTodos(usuarioId);

            return Processarlista(cirurgias);

        }

        public async Task<Result<List<Cirurgia>>> SelecionarParaHoje(Guid usuarioId)
        {
            var cirurgias = await repositorioCirurgia.SelecionarParaHoje(usuarioId);

            return Processarlista(cirurgias);

        }

        public async Task<Result<List<Cirurgia>>> SelecionarProximos30Dias(Guid usuarioId)
        {
            var cirurgias = await repositorioCirurgia.SelecionarProximos30Dias(usuarioId);

            return Processarlista(cirurgias);

        }

        public async Task<Result<List<Cirurgia>>> SelecionarUltimos30Dias(Guid usuarioId)
        {
            var cirurgias = await repositorioCirurgia.SelecionarUltimos30Dias(usuarioId);

            return Processarlista(cirurgias);

        }

        public async Task<Result<List<Cirurgia>>> SelecionarPorPeriodo(DateTime dataInicial, DateTime dataFinal, Guid usuarioId)
        {
            var cirurgias = await repositorioCirurgia.SelecionarPorPeriodo(dataInicial, dataFinal, usuarioId);

            return Processarlista(cirurgias);

        }

        public async Task<Result<List<Cirurgia>>> SelecionarCirurgiasporMedico(string CRM, Guid usuarioId)
        {
            var cirurgias = await repositorioCirurgia.ObterCirurgiasPorMedico(CRM, usuarioId);

            return Processarlista(cirurgias);
        }

    }
}
