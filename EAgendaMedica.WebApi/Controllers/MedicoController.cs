using AutoMapper;
using EAgendaMedica.Aplicacao.ModuloMedico;
using EAgendaMedica.Dominio.ModuloMedico;
using EAgendaMedica.WebApi.ViewModels.Medicos;
using eAgendaWebApi.Controllers.Compartilhado;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EAgendaMedica.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/medicos")]
    [ProducesResponseType(typeof(string[]), 500)]
    [ProducesResponseType(typeof(string[]), 401)]

    public class MedicoController : EagendaMedicaControllerBase
    {
        private ServicoMedico servicoMedico;
        public MedicoController(IMapper mapper, ServicoMedico servicoMedico) : base(mapper)
        {
            this.servicoMedico = servicoMedico;
        }

        [HttpGet]
        [ProducesResponseType((typeof(ListarMedicosViewModel)), 200)]
        public async Task<IActionResult> SelecionarTodos()
        {
            var result = await servicoMedico.SelecionarTodos(UsuarioLogado.Id);

            var medicosVM = mapper.Map<List<ListarMedicosViewModel>>(result.Value);

            return ProcessarResultado(result, medicosVM);
        }

        [HttpGet("detalhes/{id}")]
        [ProducesResponseType((typeof(VisualizarMedicoViewModel)), 200)]
        [ProducesResponseType((typeof(string[])), 400)]
        public async Task<IActionResult> SelecionarDetalhes(Guid id)
        {
            var result = await servicoMedico.SelecionarPorId(id);

            if (result.IsFailed)
                return NotFound(result);

            return ProcessarResultado(result, mapper.Map<VisualizarMedicoViewModel>(result.Value));
        }

        [HttpGet("top10")]
        [ProducesResponseType((typeof(ListarRankingMedicosViewModel)), 200)]
        public async Task<IActionResult> SelecionarTop10(DateTime dataInicial, DateTime dataFinal)
        {
            var result = await servicoMedico.SelecionarTop10(dataInicial, dataFinal, UsuarioLogado.Id);

            if (result.IsFailed)
                return NotFound(result);

            return ProcessarResultado(result, mapper.Map<List<ListarRankingMedicosViewModel>>(result.Value));
        }

        [HttpGet("crm/{crm}")]
        [ProducesResponseType((typeof(VisualizarAgendaMedicoViewModel)), 200)]
        [ProducesResponseType((typeof(string[])), 400)]
        public async Task<IActionResult> SelecionarPorCRM(string crm)
        {
            var result = await servicoMedico.SelecionarPorCRM(crm, UsuarioLogado.Id);

            if (result.IsFailed)
                return NotFound(result);

            return ProcessarResultado(result, mapper.Map<VisualizarAgendaMedicoViewModel>(result.Value));
        }

        [HttpGet("{id}")]
        [ProducesResponseType((typeof(FormMedicoViewModel)), 200)]
        [ProducesResponseType((typeof(string[])), 400)]
        public async Task<IActionResult> SelecionarPorId(Guid id)
        {
            var result = await servicoMedico.SelecionarPorId(id);

            if (result.IsFailed)
                return NotFound(result);

            return ProcessarResultado(result, mapper.Map<FormMedicoViewModel>(result.Value));
        }

        [HttpGet("status")]
        [ProducesResponseType((typeof(ListarMedicosViewModel)), 200)]
        public async Task<IActionResult> SelecionarPorStatus(bool ativo)
        {
            var result = await servicoMedico.SelecionarPorStatus(ativo, UsuarioLogado.Id);

            return ProcessarResultado(result, mapper.Map<List<ListarMedicosViewModel>>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType((typeof(FormMedicoViewModel)), 200)]
        public async Task<IActionResult> Inserir(FormMedicoViewModel medicoVM)
        {
            var medico = mapper.Map<Medico>(medicoVM);

            medico.UsuarioId = UsuarioLogado.Id;

            var result = await servicoMedico.Inserir(medico);

            return ProcessarResultado(result, mapper.Map<FormMedicoViewModel>(result.Value));
        }


        [HttpPut("{id}")]
        [ProducesResponseType((typeof(FormMedicoViewModel)), 200)]
        [ProducesResponseType((typeof(string[])), 400)]
        public async Task<IActionResult> Editar(Guid id, FormMedicoViewModel medicoVM)
        {
            var medicoResult = await servicoMedico.SelecionarPorId(id);

            if (medicoResult.IsFailed)
                return NotFound(medicoResult);

            var medico = mapper.Map(medicoVM, medicoResult.Value);

            var result = await servicoMedico.Editar(medico);

            return ProcessarResultado(result, mapper.Map<FormMedicoViewModel>(result.Value));
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var medicoResult = await servicoMedico.SelecionarPorId(id);

            if (medicoResult.IsFailed)
                return NotFound(medicoResult);

            var result = await servicoMedico.Excluir(medicoResult.Value);

            return ProcessarResultado(result, $"Medico CRM {medicoResult.Value.CRM} excluído com sucesso");
        }

        [HttpPut("alterar-status/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType((typeof(string[])), 400)]
        public async Task<IActionResult> AlterarStatus(Guid id)
        {
            var medicoResult = await servicoMedico.SelecionarPorId(id);

            if (medicoResult.IsFailed)
                return NotFound(medicoResult);

            medicoResult.Value.AlterarStatus();

            var result = await servicoMedico.Editar(medicoResult.Value);

            return ProcessarResultado(result, $"Medico CRM {result.Value.CRM} alterado com sucesso");
        }
    }
}
