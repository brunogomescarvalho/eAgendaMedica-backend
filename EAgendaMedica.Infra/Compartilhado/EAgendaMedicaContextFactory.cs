namespace EAgendaMedica.Infra.Compartilhado
{
    public class EAgendaMedicaContextFactory : IDesignTimeDbContextFactory<EAgendaMedicaDBContext>
    {
        public EAgendaMedicaDBContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            string connectionString = args.Any(arg => arg == "Testing") ?
                 "SqlServerTests" : "Postgres";

            var optionsBuilder = new DbContextOptionsBuilder<EAgendaMedicaDBContext>();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString(connectionString));

            return new EAgendaMedicaDBContext(optionsBuilder.Options);

        }
    }
}
