using AutoMapper;
using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.Dominio.ModuloMedico;
using EAgendaMedica.WebApi.ViewModels.Medicos;

namespace eAgendaWebApi.Configs.AutoMapper
{
    public class MedicoVisualizacaoCompletaMappingAction : IMappingAction<Medico, VisualizarMedicoViewModel>
    {
        public void Process(Medico source, VisualizarMedicoViewModel destination, ResolutionContext context)
        {
            destination.Atividades ??= new();

            source.TodasAtividades().ForEach(x =>
            {
                destination.Atividades.Add(new ListarAtividadesMedicoViewModel()
                {
                    Id = x.Id,
                    DataInicio = x.DataInicio.ToShortDateString(),
                    HoraInicio = x.HoraInicio.ToString(@"hh\:mm"),
                    HoraTermino = x.HoraTermino.ToString(@"hh\:mm"),
                    TipoAtividade = x is Cirurgia ? "Cirurgia" : "Consulta"
                });
            });
        }
    }
}
