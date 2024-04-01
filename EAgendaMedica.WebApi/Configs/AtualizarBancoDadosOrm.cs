using Serilog;
using EAgendaMedica.Infra.Compartilhado;

namespace EAgendaMedica.WebApi
{
    public static class AtualizarBancoDadosOrm
    {
        public static void AtualizarBancoDeDados(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var db = services.GetRequiredService<EAgendaMedicaDBContext>();

                Log.Logger.Information("Atualizando a banco de dados do e-Agenda...");

                var migracaoRealizada = MigradorBancoDadoseAgenda.AtualizarBancoDados(db);

                if (migracaoRealizada)               
                    Log.Logger.Information("Banco de dados atualizado");                          
                
                else
                    Log.Logger.Information("Nenhuma migração pendente");
            }
        }
    }
}
