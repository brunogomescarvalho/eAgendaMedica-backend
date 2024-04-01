using EAgendaMedica.WebApi.Configs.AutoMapper.Profiles;

namespace eAgendaWebApi.Configs.AutoMapper
{
    public static class AutoMapperConfigExtension
    {
        public static void ConfigurarAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(opt =>
            {
                opt.AddProfile<CirurgiaProfile>();
                opt.AddProfile<ConsultaProfile>();
                opt.AddProfile<MedicoProfile>();
            });
        }
    }
}
