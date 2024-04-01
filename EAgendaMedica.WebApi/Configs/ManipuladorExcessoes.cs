using System.Text.Json;

namespace eAgendaWebApi.Configs
{
    public class ManipuladorExcessoes
    {

        readonly RequestDelegate requestDelegate;

        public ManipuladorExcessoes(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await requestDelegate(ctx);
            }
            catch (Exception ex)
            {
                var codErro = ex.Source == "Microsoft.AspNetCore.Authentication.Core" ? 401 : 500;

                ctx.Response.StatusCode = codErro;
                ctx.Response.ContentType = "application/json";

                var resposta = new
                {
                    sucesso = false,
                    erros = codErro == 401 ? "Usuário não logado" : ex.Message
                };

                await ctx.Response.WriteAsync(JsonSerializer.Serialize(resposta));
            }
        }
    }
}
