using AutoMapper;
using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.WebApi.ViewModels.Consultas;
using EAgendaMedica.WebApi.ViewModels.Compartilhado;

namespace EAgendaMedica.WebApi.Configs.AutoMapper.Profiles
{
    public class ConsultaProfile : Profile
    {
        public ConsultaProfile()
        {
            CreateMap<Consulta, FormConsultaViewModel>();

            CreateMap<Consulta, ListarAtividadeViewModel>()
                .BeforeMap((src, dest) => src.AtualizarInformacoes(src))
                .ForMember(dest => dest.DataInicio, opt => opt.MapFrom(x => x.DataInicio.ToShortDateString()))
                .ForMember(dest => dest.HoraInicio, opt => opt.MapFrom(x => x.HoraInicio.ToString(@"hh\:mm")))
                .ForMember(dest => dest.HoraTermino, opt => opt.MapFrom(x => x.HoraTermino.ToString(@"hh\:mm")));


            CreateMap<Consulta, VisualizarConsultaViewModel>()
                 .BeforeMap((src, dest) => src.AtualizarInformacoes(src))
                 .ForMember(dest => dest.DataInicio, opt => opt.MapFrom(x => x.DataInicio.ToShortDateString()))
                 .ForMember(dest => dest.DataTermino, opt => opt.MapFrom(x => x.DataTermino.ToShortDateString()))
                 .ForMember(dest => dest.HoraInicio, opt => opt.MapFrom(x => x.HoraInicio.ToString(@"hh\:mm")))
                 .ForMember(dest => dest.HoraTermino, opt => opt.MapFrom(x => x.HoraTermino.ToString(@"hh\:mm")));
               

            CreateMap<FormConsultaViewModel, Consulta>()
                .ForMember(x => x.MedicoId, opt => opt.Ignore())
                .ForMember(origem => origem.DataInicio, opt => opt.MapFrom(x => x.DataInicio.ToUniversalTime()))
                .AfterMap<InserirMedicoMappingAction>();
        }
    }
}

