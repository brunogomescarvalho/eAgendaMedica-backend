namespace EAgendaMedica.Dominio.Servicos
{
    public class VerificarAgendaMedico
    {
        private Atividade atividadeParaVerificar;

        public VerificarAgendaMedico(Atividade atividadeParaVerificar)
        {
            this.atividadeParaVerificar = atividadeParaVerificar;
          
        }

        public bool VerificarAgenda(List<Atividade> atividadesDoDia)
        {
            var encontrado = atividadesDoDia.Where(x => x.Equals(atividadeParaVerificar) == false &&

            TemConflito(x, atividadeParaVerificar)).FirstOrDefault();

            return encontrado == null;
        }
        private static bool TemConflito(Atividade outro, Atividade paraVerificar)
        {
            return
              (outro.HoraInicio >= paraVerificar.HoraInicio && outro.HoraInicio <= paraVerificar.HoraTermino)

           || (outro.HoraTermino >= paraVerificar.HoraInicio && outro.HoraTermino <= paraVerificar.HoraTermino)

           || (outro.HoraInicio <= paraVerificar.HoraInicio && outro.HoraTermino >= paraVerificar.HoraTermino)

           || (outro.HoraInicio >= paraVerificar.HoraInicio && outro.HoraTermino <= paraVerificar.HoraTermino);

        }
    }

}
