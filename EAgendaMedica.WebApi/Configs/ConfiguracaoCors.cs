namespace EAgendaMedica.WebApi.Configs
{
    public static class ConfiguracaoCors
    {
        public static void ConfigurarCors(this IServiceCollection services)
        {
            services.AddCors(options =>
             {
                 options.AddPolicy("Desenvolvimento",
                 x => x.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());
             });
        }
    }
}
