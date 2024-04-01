using EAgendaMedica.Aplicacao.ModuloCirurgia;
using EAgendaMedica.Aplicacao.ModuloConsulta;
using EAgendaMedica.Aplicacao.ModuloMedico;
using EAgendaMedica.Dominio;
using EAgendaMedica.Dominio.Copartilhado;
using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.Infra.Compartilhado;
using EAgendaMedica.Infra.ModuloCirurgia;
using EAgendaMedica.Infra.ModuloConsulta;
using EAgendaMedica.Infra.ModuloMedico;
using EAgendaMedica.WebApi;
using EAgendaMedica.WebApi.Configs.AutoMapper;
using EAgendaMedica.WebApi.Services;
using eAgendaWebApi.Configs.AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace eAgendaWebApi.Configs
{
    public static class ConfigurarInjecaoDependencia
    {
        public static void InjetarDependencias(this IServiceCollection service, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("Postgres")!;

            service.AddDbContext<IContextoPersistencia, EAgendaMedicaDBContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql(connectionString);
            });

            service.AddTransient<ServicoCirurgia>();
            service.AddTransient<IRepositorioCirurgia, RepositorioCirurgia>();

            service.AddTransient<ServicoConsulta>();
            service.AddTransient<IRepositorioConsulta, RepositorioConsulta>();

            service.AddTransient<ServicoMedico>();
            service.AddTransient<IRepositorioMedico, RepositorioMedico>();

            service.AddTransient<JwtService>();

            service.AddTransient<InserirMedicoMappingAction>();
            service.AddTransient<InserirMedicosMappingAction>();
            service.AddTransient<MedicoVisualizacaoCompletaMappingAction>();
            service.AddTransient<MedicoAgendaDoDiaMappingAction>();
        }
    }
}
