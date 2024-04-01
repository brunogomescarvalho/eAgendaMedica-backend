using AutoMapper;
using EAgendaMedica.Aplicacao.ModuloCirurgia;
using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.WebApi.ViewModels.Compartilhado;
using EAgendaMedica.WebApi.ViewModels.Cirurgias;
using eAgendaWebApi.Controllers.Compartilhado;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EAgendaMedica.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/cirurgias")]
    [ProducesResponseType(typeof(string[]), 401)]
    [ProducesResponseType(typeof(string[]), 500)]

    public class CirurgiaController : EagendaMedicaControllerBase
    {
        private ServicoCirurgia servicoCirurgia;

        public CirurgiaController(ServicoCirurgia servicoCirurgia, IMapper mapper) : base(mapper)
        {
            this.servicoCirurgia = servicoCirurgia;
        }

        [HttpGet]
        [ProducesResponseType((typeof(ListarAtividadeViewModel)), 200)]       
        public async Task<IActionResult> SelecionarTodos()
        {
            
            var result = await servicoCirurgia.SelecionarTodos(UsuarioLogado.Id);

            return ProcessarResultado(result, mapper.Map<List<ListarAtividadeViewModel>>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType((typeof(VisualizarCirurgiaViewModel)), 200)]    
        public async Task<IActionResult> Inserir(FormCirurgiaViewModel CirurgiaVM)
        {
            var cirurgia = mapper.Map<Cirurgia>(CirurgiaVM);

            cirurgia.UsuarioId = UsuarioLogado.Id;

            var result = await servicoCirurgia.Inserir(cirurgia);

            var novaCirurgiaVM = mapper.Map<VisualizarCirurgiaViewModel>(result.Value);

            return ProcessarResultado(result, novaCirurgiaVM);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((typeof(VisualizarCirurgiaViewModel)), 200)]       
        public async Task<IActionResult> Editar(Guid id, FormCirurgiaViewModel CirurgiaVM)
        {
            var result = await servicoCirurgia.SelecionarPorId(id);

            if (result.IsFailed)
                return NotFound(result);

            var Cirurgia = mapper.Map(CirurgiaVM, result.Value);

            var resultUpdate = await servicoCirurgia.Editar(Cirurgia);

            var CirurgiaEditadaVM = mapper.Map<VisualizarCirurgiaViewModel>(resultUpdate.Value);

            return ProcessarResultado(resultUpdate, CirurgiaEditadaVM);

        }

        [HttpGet("medico/{crm}")]
        [ProducesResponseType((typeof(ListarAtividadeViewModel)), 200)]  
        public async Task<IActionResult> SelecionarCirurgiasPorMedico(string crm)
        {
            var result = await servicoCirurgia.SelecionarCirurgiasporMedico(crm, UsuarioLogado.Id);

            return ProcessarResultado(result, mapper.Map<List<ListarAtividadeViewModel>>(result.Value));
        }


        [HttpGet("periodo")]
        [ProducesResponseType((typeof(ListarAtividadeViewModel)), 200)]      
        public async Task<IActionResult> SelecionarCirurgiasPorPeriodo([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
        {
            var result = await servicoCirurgia.SelecionarPorPeriodo(dataInicio, dataFim, UsuarioLogado.Id);

            return ProcessarResultado(result, mapper.Map<List<ListarAtividadeViewModel>>(result.Value));
        }


        [HttpGet("{id}")]
        [ProducesResponseType((typeof(FormCirurgiaViewModel)), 200)]    
        public async Task<IActionResult> SelecionarCirurgiasId(Guid id)
        {
            var result = await servicoCirurgia.SelecionarPorId(id);

            return ProcessarResultado(result, mapper.Map<FormCirurgiaViewModel>(result.Value));
        }


        [HttpGet("hoje")]
        [ProducesResponseType((typeof(ListarAtividadeViewModel)), 200)]     
        public async Task<IActionResult> SelecionarCirurgiasParaHoje()
        {
            var result = await servicoCirurgia.SelecionarParaHoje(UsuarioLogado.Id);

            return ProcessarResultado(result, mapper.Map<List<ListarAtividadeViewModel>>(result.Value));
        }

        [HttpGet("ultimos-30-dias")]
        [ProducesResponseType((typeof(ListarAtividadeViewModel)), 200)]      
        public async Task<IActionResult> SelecionarCirurgiasPassadas()
        {
            var result = await servicoCirurgia.SelecionarUltimos30Dias(UsuarioLogado.Id);

            return ProcessarResultado(result, mapper.Map<List<ListarAtividadeViewModel>>(result.Value));
        }


        [HttpGet("proximos-30-dias")]
        [ProducesResponseType((typeof(ListarAtividadeViewModel)), 200)]    
        public async Task<IActionResult> SelecionarCirurgiasFuturas()
        {
            var result = await servicoCirurgia.SelecionarProximos30Dias(UsuarioLogado.Id);

            return ProcessarResultado(result, mapper.Map<List<ListarAtividadeViewModel>>(result.Value));
        }


        [HttpGet("detalhes/{id}")]
        [ProducesResponseType((typeof(VisualizarCirurgiaViewModel)), 200)]
        [ProducesResponseType(typeof(string[]), 400)]
        public async Task<IActionResult> VisualizarCirurgiaCompleta(Guid id)
        {
            var result = await servicoCirurgia.SelecionarPorId(id);

            return ProcessarResultado(result, mapper.Map<VisualizarCirurgiaViewModel>(result.Value));
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string[]), 400)]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var result = await servicoCirurgia.SelecionarPorId(id);

            if (result.IsFailed)
                return NotFound(result);

            var resultDelete = await servicoCirurgia.Excluir(result.Value);

            return ProcessarResultado((Result<Cirurgia>)resultDelete, $"Cirurgia excluída com sucesso");
        }
    }
}

