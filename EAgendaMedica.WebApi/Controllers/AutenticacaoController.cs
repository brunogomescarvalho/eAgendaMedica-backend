using AutoMapper;
using eAgendaWebApi.Controllers.Compartilhado;
using Microsoft.AspNetCore.Mvc;
using EAgendaMedica.Dominio.ModuloAutenticacao;
using EAgendaMedica.Servico.ModuloAutenticacao;
using EAgendaMedica.WebApi.Services;
using EAgendaMedica.WebApi.ViewsModels.AutenticacaoVM;

namespace EAgendaMedica.WebApi.Controllers
{
    [Route("api/conta")]
    [ApiController]
    public class AutenticacaoController : EagendaMedicaControllerBase
    {
        private readonly ServicoAutenticacao servicoAutenticacao;

        private readonly JwtService jwtService;

        public AutenticacaoController(ServicoAutenticacao servicoAutenticacao, JwtService jwtService, IMapper mapper) : base(mapper)
        {
            this.servicoAutenticacao = servicoAutenticacao;
            this.jwtService = jwtService;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> RegistrarUsuario(RegistrarUsuarioViewModel usuarioVM)
        {
            var usuario = new Usuario
            {
                UserName = usuarioVM.Email,
                Email = usuarioVM.Email,
                Nome = usuarioVM.Nome
            };

            var usuarioResult = await servicoAutenticacao.RegistrarUsuario(usuario, usuarioVM.Senha);

            if (usuarioResult.IsFailed)
                return BadRequest(usuarioResult);

            return Ok(jwtService.GerarJwt(usuarioResult.Value));
        }


        [HttpPost("autenticar")]
        public async Task<ActionResult> AutenticarUsuario(AutenticarUsuarioViewModel usuarioVM)
        {
            var usuarioResult = await servicoAutenticacao.AutenticarUsuario(usuarioVM.Email, usuarioVM.Senha);

            if (usuarioResult.IsFailed)
                return BadRequest(usuarioResult);

            return Ok(jwtService.GerarJwt(usuarioResult.Value));
        }

        [HttpPost("sair")]
        public async Task<ActionResult> Sair()
        {
            await servicoAutenticacao.Sair(UsuarioLogado.Email);

            return Ok( $"Sessão do usuário {UsuarioLogado.Email} removida com sucesso");
        }
    }
}
