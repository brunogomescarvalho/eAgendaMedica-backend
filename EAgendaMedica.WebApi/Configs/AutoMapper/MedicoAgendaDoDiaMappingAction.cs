using AutoMapper;
using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.Dominio.ModuloMedico;
using EAgendaMedica.WebApi.ViewModels.Medicos;

namespace eAgendaWebApi.Configs.AutoMapper
{
    public class MedicoAgendaDoDiaMappingAction : IMappingAction<Medico, VisualizarAgendaMedicoViewModel>
    {
        public void Process(Medico source, VisualizarAgendaMedicoViewModel destination, ResolutionContext context)
        {
            destination.Atividades ??= new();

            source.AtividadesDoDia(DateTime.Now).ForEach(x =>
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
