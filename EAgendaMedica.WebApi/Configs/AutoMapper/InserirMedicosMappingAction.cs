using AutoMapper;
using EAgendaMedica.Dominio;
using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.WebApi.ViewModels.Cirurgias;

namespace EAgendaMedica.WebApi.Configs.AutoMapper
{
    public class InserirMedicosMappingAction : IMappingAction<FormCirurgiaViewModel, Cirurgia>
    {
        public InserirMedicosMappingAction(IRepositorioMedico repositorioMedico)
        {
            RepositorioMedico = repositorioMedico;
        }

        public IRepositorioMedico RepositorioMedico { get; }

        public void Process(FormCirurgiaViewModel source, Cirurgia destination, ResolutionContext context)
        {

            if(destination.Medicos != null)
            {
                var medicosParaRemover = destination.Medicos.FindAll(x => source.MedicosIds!.Contains(x.Id) == false);

                if (medicosParaRemover.Any())
                    destination.Medicos.RemoveAll(x => medicosParaRemover.Contains(x));
            }

            destination.Medicos = RepositorioMedico.SelecionarMuitos(source.MedicosIds!).Result;
        }
    }

}
