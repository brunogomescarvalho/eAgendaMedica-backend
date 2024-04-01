using AutoMapper;
using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.WebApi.ViewModels.Consultas;
using EAgendaMedica.Dominio;

namespace EAgendaMedica.WebApi.Configs.AutoMapper
{
    public class InserirMedicoMappingAction : IMappingAction<FormConsultaViewModel, Consulta>
    {
        public InserirMedicoMappingAction(IRepositorioMedico repositorioMedico)
        {
            RepositorioMedico = repositorioMedico;
        }

        public IRepositorioMedico RepositorioMedico { get; }

        public void Process(FormConsultaViewModel source, Consulta destination, ResolutionContext context)
        {
            destination.Medico = RepositorioMedico.SelecionarPorId(source.MedicoId).Result;


        }
    }
}

