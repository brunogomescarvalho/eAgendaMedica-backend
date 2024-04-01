using EAgendaMedica.Infra.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace EAgendaMedica.WebApi
{
    public class MigradorBancoDadoseAgenda
    {
        public static bool AtualizarBancoDados(EAgendaMedicaDBContext db)
        {
            var migracoesPendentes = db.Database.GetPendingMigrations();

            if (migracoesPendentes.Any())
            {
                db.Database.Migrate();

                return true;
            }

            return false;
        }
    }
}
