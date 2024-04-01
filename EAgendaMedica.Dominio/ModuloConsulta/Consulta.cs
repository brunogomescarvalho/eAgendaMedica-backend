using EAgendaMedica.Dominio.ModuloAutenticacao;
using EAgendaMedica.Dominio.ModuloMedico;
using EAgendaMedica.Dominio.Servicos;

namespace EAgendaMedica.Dominio.ModuloConsulta
{
    public class Consulta : Atividade
    {
        public Guid MedicoId { get; set; }

        private Medico medico;
        public Medico Medico
        {
            get => medico;
            set
            {
                AdicionarMedico(value);
            }
        }
        public Consulta() { }

        public Consulta(DateTime data, TimeSpan horaInicio, int duracao, Medico medico) : base(data, horaInicio, duracao)
        {
            Medico = medico;
        }

        public Consulta(DateTime data, TimeSpan horaInicio, int duracao, Medico medico, Usuario usuario) : this(data, horaInicio, duracao, medico)
        {
            Usuario = usuario;
        }

        public void AdicionarMedico(Medico medico)
        {
            if (medico != null)
            {
                MedicoId = medico.Id;
                this.medico = medico;
                medico.AdicionarConsulta(this);
            }

        }

        public override bool VerificarDescansoMedico()
        {
            return new VerificadorDescanso(this).Verificar(Medico);
        }

        public bool VerificarAgendaMedico()
        {
            return new VerificarAgendaMedico(this).VerificarAgenda(medico.AtividadesDoDia(this.DataInicio));
        }

        public override string ToString()
        {
            var passouDaMeiaNoite = DataInicio.Date != DataTermino.Date;

            return $"Data: {DataInicio:d} - {HoraInicio} -- Término: {(passouDaMeiaNoite ? DataTermino.ToShortDateString() : "")} - {HoraTermino} -- Dr: {Medico}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Consulta consulta &&
                   Id.Equals(consulta.Id) &&
                   DataInicio == consulta.DataInicio &&
                   HoraInicio.Equals(consulta.HoraInicio) &&
                   DuracaoEmMinutos == consulta.DuracaoEmMinutos &&
                   HoraTermino.Equals(consulta.HoraTermino) &&
                   DataTermino == consulta.DataTermino &&
                   EqualityComparer<Medico>.Default.Equals(Medico, consulta.Medico);
        }
    }
}

