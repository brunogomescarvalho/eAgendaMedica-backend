using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using EAgendaMedica.WebApi.ViewsModels.AutenticacaoVM;
using System.Security.Claims;

namespace eAgendaWebApi.Controllers.Compartilhado
{

    [ApiController]
    public abstract class EagendaMedicaControllerBase : ControllerBase
    {
        protected IMapper mapper;
        protected EagendaMedicaControllerBase(IMapper mapper)
        {
            this.mapper = mapper;
        }

        private UsuarioTokenViewModel usuario;

        public UsuarioTokenViewModel UsuarioLogado
        {
            get
            {
                if (EstaAutenticado())
                {
                    usuario = new UsuarioTokenViewModel();

                    var id = Request?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!string.IsNullOrEmpty(id))
                        usuario.Id = Guid.Parse(id);

                    var nome = Request?.HttpContext?.User?.FindFirst(ClaimTypes.GivenName)?.Value;

                    if (!string.IsNullOrEmpty(nome))
                        usuario.Nome = nome;

                    var email = Request?.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

                    if (!string.IsNullOrEmpty(email))
                        usuario.Email = email;

                    return usuario;
                }

                return null!;
            }
        }

        private bool EstaAutenticado()
        {
            if (Request?.HttpContext?.User?.Identity != null)
                return true;

            return false;
        }


        protected BadRequestObjectResult BadRequest<T>(Result<T> result)
        {
            var resposta = new
            {
                sucesso = false,
                erros = result.Reasons.Select(m => m.Message).ToArray(),
            };

            return new BadRequestObjectResult(resposta);
        }

        protected NotFoundObjectResult NotFound<T>(Result<T> result)
        {
            var resposta = new
            {
                sucesso = false,
                erros = result.Reasons.Select(m => m.Message).ToArray()
            };

            return new NotFoundObjectResult(resposta);
        }

        public override OkObjectResult Ok(object? obj)
        {
            var resposta = new
            {
                sucesso = true,
                dados = obj
            };

            return new OkObjectResult(resposta);
        }

        protected IActionResult ProcessarResultado<T>(Result<T> result, object obj)
        {
            if (result.IsSuccess)
                return Ok(obj);

            else if (result.IsFailed)
                return BadRequest(result);

            return NotFound(result);
        }      
    }
}
