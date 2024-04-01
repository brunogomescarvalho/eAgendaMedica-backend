using EAgendaMedica.Infra.Compartilhado;

namespace EAgendaMedicaOrmTests
{
    [TestClass]
    public abstract class TestsBase
    {

        EAgendaMedicaDBContext dbContext { get; set; }


        public void ObterContexto()
        {

        }
    }
}