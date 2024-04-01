

using AutoMapper;
using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.WebApi.ViewModels.Cirurgias;
using EAgendaMedica.WebApi.ViewModels.Compartilhado;

namespace EAgendaMedica.WebApi.Configs.AutoMapper.Profiles
{
    public class CirurgiaProfile : Profile
    {
        public CirurgiaProfile()
        {
            CreateMap<Cirurgia, FormCirurgiaViewModel>()
                .ForMember(origem => origem.MedicosIds, opt => opt.MapFrom(x => x.Medicos.Select(x => x.Id)));

            CreateMap<Cirurgia, ListarAtividadeViewModel>()
                 .BeforeMap((src, dest) => src.AtualizarInformacoes(src))
                 .ForMember(dest => dest.DataInicio, opt => opt.MapFrom(x => x.DataInicio.ToShortDateString()))
                 .ForMember(dest => dest.HoraInicio, opt => opt.MapFrom(x => x.HoraInicio.ToString(@"hh\:mm")))
                 .ForMember(dest => dest.HoraTermino, opt => opt.MapFrom(x => x.HoraTermino.ToString(@"hh\:mm"))); ;

            CreateMap<Cirurgia, VisualizarCirurgiaViewModel>()
                 .BeforeMap((src, dest) => src.AtualizarInformacoes(src))
                 .ForMember(dest => dest.DataInicio, opt => opt.MapFrom(x => x.DataInicio.ToShortDateString()))
                 .ForMember(dest => dest.DataTermino, opt => opt.MapFrom(x => x.DataTermino.ToShortDateString()))
                 .ForMember(dest => dest.HoraInicio, opt => opt.MapFrom(x => x.HoraInicio.ToString(@"hh\:mm")))
                 .ForMember(dest => dest.HoraTermino, opt => opt.MapFrom(x => x.HoraTermino.ToString(@"hh\:mm"))); ;

            CreateMap<FormCirurgiaViewModel, Cirurgia>()
                  .ForMember(origem => origem.Medicos, opt => opt.Ignore())
                  .ForMember(origem=>origem.DataInicio, opt =>opt.MapFrom(x=>x.DataInicio.ToUniversalTime()))
                  .AfterMap<InserirMedicosMappingAction>();

        }
    }

}
