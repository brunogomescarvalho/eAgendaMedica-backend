using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

public class SerilogActionFilter : IActionFilter
{
    private object? nomeEndpoint;
    private object? nomeModulo;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        nomeEndpoint = context.RouteData.Values["action"];

        nomeModulo = context.RouteData.Values["controller"];

        Log.Logger.Information($"[Módulo de {nomeModulo}] -> Tentando {nomeEndpoint}...");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        nomeEndpoint = context.RouteData.Values["action"];

        nomeModulo = context.RouteData.Values["controller"];

        if (context.Exception == null)
        {
            Log.Logger.Information($"[Módulo de {nomeModulo}] -> {nomeEndpoint} executado com sucesso");
        }
        else
        {
            Log.Logger.Information($"[Módulo de {nomeModulo}] -> Falha ao executar {nomeEndpoint}...");
        }
    }

}