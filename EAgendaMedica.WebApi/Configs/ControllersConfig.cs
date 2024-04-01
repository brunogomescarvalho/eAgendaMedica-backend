namespace eAgendaWebApi.Configs
{
    public static class ControllersConfig
    {
        public static void ConfigurarControladores(this IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                opt.Filters.Add<SerilogActionFilter>();
            })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new TimeSpanToStringConverter());
                opt.JsonSerializerOptions.Converters.Add(new DateTimeToStringConverter());
            });
        }
    }
}