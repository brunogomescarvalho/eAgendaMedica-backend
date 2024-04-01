using Serilog;

namespace eAgendaWebApi.Configs
{
    public static class ConfigurarSerilog
    {
        public static void ConfigurarLoggin(this IServiceCollection services, ILoggingBuilder logging)
        {
            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Information()
             .Enrich.FromLogContext()
             .WriteTo.Console()
             .CreateLogger();

            Log.Logger.Information("Iniciando aplicação...");

            logging.ClearProviders();

            services.AddSerilog(Log.Logger);
        }
    }
}