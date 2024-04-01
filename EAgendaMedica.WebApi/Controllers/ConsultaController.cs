using AutoMapper;
using EAgendaMedica.Aplicacao.ModuloConsulta;
using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.WebApi.ViewModels.Compartilhado;
using EAgendaMedica.WebApi.ViewModels.Consultas;
using eAgendaWebApi.Controllers.Compartilhado;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EAgendaMedica.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/consultas")]
    [ProducesResponseType(typeof(string[]), 500)]
    [ProducesResponseType(typeof(string[]), 401)]

    public class ConsultaController : EagendaMedicaControllerBase
    {
        ServicoConsulta servicoConsulta;
        public ConsultaController(ServicoConsulta servicoConsulta, IMapper mapper) : base(mapper)
        {
            this.servicoConsulta = servicoConsulta;
        }

        [HttpGet]
        [ProducesResponseType((typeof(ListarAtividadeViewModel)), 200)]
        public async Task<IActionResult> SelecionarTodos()
        {
            var result = await servicoConsulta.SelecionarTodos(UsuarioLogado.Id);

            return ProcessarResultado(result, mapper.Map<List<ListarAtividadeViewModel>>(result.Value));
        }

        [HttpGet("{id}")]
        [ProducesResponseType((typeof(FormConsultaViewModel)), 200)]
        [ProducesResponseType(typeof(string[]), 400)]
        public async Task<IActionResult> SelecionarConsultasId(Guid id)
        {
            var result = await servicoConsulta.SelecionarPorId(id);

            return ProcessarResultado(result, mapper.Map<FormConsultaViewModel>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType((typeof(VisualizarConsultaViewModel)), 200)]
        public async Task<IActionResult> Inserir(FormConsultaViewModel consultaVM)
        {
            var consulta = mapper.Map<Consulta>(consultaVM);

            consulta.UsuarioId = UsuarioLogado.Id;

            var result = await servicoConsulta.Inserir(consulta);

            var novaConsultaVM = mapper.Map<VisualizarConsultaViewModel>(result.Value);

            return ProcessarResultado(result, novaConsultaVM);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((typeof(VisualizarConsultaViewModel)), 200)]
        public async Task<IActionResult> Editar(Guid id, FormConsultaViewModel consultaVM)
        {
            var result = await servicoConsulta.SelecionarPorId(id);

            if (result.IsFailed)
                return NotFound(result);

            var consulta = mapper.Map(consultaVM, result.Value);

            var resultUpdate = await servicoConsulta.Editar(consulta);

            var consultaEditadaVM = mapper.Map<VisualizarConsultaViewModel>(resultUpdate.Value);

            return ProcessarResultado(resultUpdate, consultaEditadaVM);

        }

        [HttpGet("medico/{crm}")]
        [ProducesResponseType((typeof(ListarAtividadeViewModel)), 200)]
        public async Task<IActionResult> SelecionarConsultasPorMedico(string crm)
        {
            var result = await servicoConsulta.SelecionarConsultasPorMedico(crm, UsuarioLogado.Id);

            return ProcessarResultado(result, mapper.Map<List<ListarAtividadeViewModel>>(result.Value));
        }


        [HttpGet("periodo")]
        [ProducesResponseType((typeof(ListarAtividadeViewModel)), 200)]
        public async Task<IActionResult> SelecionarConsultasPorPeriodo([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
        {
            var result = await servicoConsulta.SelecionarPorPeriodo(dataInicio, dataFim, UsuarioLogado.Id);

            return ProcessarResultado(result, mapper.Map<List<ListarAtividadeViewModel>>(result.Value));
        }


        [HttpGet("hoje")]
        [ProducesResponseType((typeof(ListarAtividadeViewModel)), 200)]
        public async Task<IActionResult> SelecionarConsultasParaHoje()
        {
            var result = await servicoConsulta.SelecionarParaHoje(UsuarioLogado.Id);

            return ProcessarResultado(result, mapper.Map<List<ListarAtividadeViewModel>>(result.Value));
        }

        [HttpGet("ultimos-30-dias")]
        [ProducesResponseType((typeof(ListarAtividadeViewModel)), 200)]
        public async Task<IActionResult> SelecionarConsultasPassadas()
        {
            var result = await servicoConsulta.SelecionarUltimos30Dias(UsuarioLogado.Id);

            return ProcessarResultado(result, mapper.Map<List<ListarAtividadeViewModel>>(result.Value));
        }


        [HttpGet("proximos-30-dias")]
        [ProducesResponseType((typeof(ListarAtividadeViewModel)), 200)]
        public async Task<IActionResult> SelecionarConsultasFuturas()
        {
            var result = await servicoConsulta.SelecionarProximos30Dias(UsuarioLogado.Id);

            return ProcessarResultado(result, mapper.Map<List<ListarAtividadeViewModel>>(result.Value));
        }


        [HttpGet("detalhes/{id}")]
        [ProducesResponseType((typeof(VisualizarConsultaViewModel)), 200)]
        public async Task<IActionResult> VisualizarConsultaCompleta(Guid id)
        {
            var result = await servicoConsulta.SelecionarPorId(id);

            return ProcessarResultado(result, mapper.Map<VisualizarConsultaViewModel>(result.Value));
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var result = await servicoConsulta.SelecionarPorId(id);

            if (result.IsFailed)
                return NotFound(result);

            var resultDelete = await servicoConsulta.Excluir(result.Value);

            return ProcessarResultado((Result<Consulta>)resultDelete, $"Consulta excluída com sucesso");
        }
    }
}
