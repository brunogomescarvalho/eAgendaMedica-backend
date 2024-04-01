using EAgendaMedica.Dominio.ModuloAutenticacao;
using System.ComponentModel.DataAnnotations.Schema;
using Taikandi;

namespace EAgendaMedica.Dominio.Compartilhado
{
    [NotMapped]
    public abstract class Atividade
    {
        public Guid UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        public Guid Id { get; set; }

        public DateTime DataInicio
        {
            get => data;
            set
            {
                data = value.Date;
            }
        }

        public TimeSpan HoraInicio
        {
            get => horaInicio;
            set
            {
                data = data.Add(value);
                horaInicio = value;
            }
        }

        public int DuracaoEmMinutos
        {
            get => duracaoEmMinutos;
            set
            {
                duracaoEmMinutos = value;
                dataHoraTermino = data.AddMinutes(duracaoEmMinutos);
            }
        }

        public TimeSpan HoraTermino { get => DataTermino.TimeOfDay; }
        public DateTime DataTermino { get => dataHoraTermino; }

        private TimeSpan horaInicio;

        private DateTime data;

        private DateTime dataHoraTermino;

        private int duracaoEmMinutos;


        public Atividade(DateTime data, TimeSpan horaInicio, int duracao) : this()
        {
            this.DataInicio = data;
            this.HoraInicio = horaInicio;
            this.DuracaoEmMinutos = duracao;
        }

        public void AtualizarInformacoes(Atividade atividade)
        {
            this.DataInicio = atividade.DataInicio;
            this.HoraInicio = atividade.HoraInicio;
            this.DuracaoEmMinutos = atividade.DuracaoEmMinutos;
        }

        public abstract bool VerificarDescansoMedico();

        public Atividade()
        {
            Id = SequentialGuid.NewGuid();
        }

    }
}



