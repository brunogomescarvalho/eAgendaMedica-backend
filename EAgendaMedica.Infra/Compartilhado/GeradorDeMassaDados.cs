using EAgendaMedica.Dominio.ModuloCirurgia;
using EAgendaMedica.Infra.Compartilhado;
using EAgendaMedica.Dominio.ModuloConsulta;
using EAgendaMedica.Dominio.ModuloMedico;
using EAgendaMedica.Dominio;
using Serilog;
using Microsoft.AspNetCore.Identity;
using EAgendaMedica.Dominio.ModuloAutenticacao;

namespace EAgendaMedica.ConsoleApp
{
    public class GeradorDeMassaDados

    {
        readonly EAgendaMedicaDBContext dbContext;
        readonly IRepositorioConsulta resConsulta;
        readonly IRepositorioCirurgia resCirurgia;
        readonly IRepositorioMedico resMed;
        private  Usuario usuario;
        UserManager<Usuario> userManager;

        public GeradorDeMassaDados(
            UserManager<Usuario> userManager,
            EAgendaMedicaDBContext dbContext, 
            IRepositorioConsulta resConsulta, 
            IRepositorioCirurgia resCirurgia, 
            IRepositorioMedico resMed)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.resConsulta = resConsulta;
            this.resCirurgia = resCirurgia;
            this.resMed = resMed;
         
        }

        private async Task GerarUsuario()
        {
            var usuario = new Usuario()
            {
                UserName = "admin@gmail.com",
                Nome = "admin",
                Email = "admin@gmail.com",
            };

           var result  = await this.userManager.CreateAsync(usuario, "Aaa@123");

            if (result.Succeeded == false)
                throw new Exception("Falha ao gerar usuario");

            this.usuario = usuario;
        }

        public async Task GerarMassaDeDados()
        {
            try
            {
                await GerarUsuario();

                LimparTabelas(dbContext);

                var medicos = GerarMedicos(30);

                foreach (var item in medicos)
                {
                    await resMed.Inserir(item);

                    await dbContext.SaveChangesAsync();
                }

                var medicoscrmCadastrado = await resMed.SelecionarTodos(usuario.Id);

                var consultas = GerarConsultas(20, medicoscrmCadastrado);

                var cirurgias = GerarCirurgias(20, medicoscrmCadastrado);

                foreach (var item in consultas)
                {
                    await resConsulta.Inserir(item);
                }

                foreach (var item in cirurgias)
                {
                    await resCirurgia.Inserir(item);
                }

                await dbContext.SaveChangesAsync();


                var qtdCol = dbContext.Set<Consulta>().Count();
                var qtdCir = dbContext.Set<Cirurgia>().Count();
                var qtdMedico = dbContext.Set<Medico>().Count();

                Log.Logger.Information($"Gerado massa de dados com {qtdCir + qtdCol + qtdMedico} registros");
            }
            catch (Exception ex)
            {
                Log.Error("Não foi possivel gerar dados. Exception: " + ex.Message);
            }
        }



        private List<Medico> GerarMedicos(int v)
        {
            var medicos = new List<Medico>();

            List<string> crmCadastrado = new();

            for (int i = 0; i < v; i++)
            {
                string crm;

                while (true)
                {
                    crm = "";

                    while (crm.Length != 5)
                        crm += new Random().Next(0, 5).ToString();

                    if (crmCadastrado.Contains(crm))
                        continue;

                    crmCadastrado.Add(crm);

                    if (i < nomes.Count)
                        medicos.Add(new Medico(nomes[i], $"{crm}-SC", usuario));

                    else
                        medicos.Add(new Medico($"Medico-{i}", $"{crm}-SC", usuario));

                    break;

                }
            }

            return medicos;
        }

        private List<Consulta> GerarConsultas(int v, List<Medico> medicos)
        {
            var consultas = new List<Consulta>();

            var hora = 0;

            var data = DateTime.Now.AddDays(-5);

            for (int i = 0; i < v; i++)
            {
                if (i % 2 != 0)
                    data = data.AddDays(1);

                if (hora >= 24)
                {
                    hora = 1;
                }

                var horaInicial = TimeSpan.Parse($"{hora += 1}:00");

                var index = new Random().Next(0, medicos.Count - 1);

                Medico medico = medicos[index];

                var consulta = new Consulta(data, horaInicial, 120, medico, usuario);

                var ehValida = consulta.VerificarDescansoMedico() && consulta.VerificarAgendaMedico();

                if (ehValida == true)
                    consultas.Add(consulta);

                hora += 2;
            }

            return consultas;

        }

        private List<Cirurgia> GerarCirurgias(int v, List<Medico> medicos)
        {
            var cirurgias = new List<Cirurgia>();

            var hora = 2;

            var data = DateTime.Now.AddDays(-5);

            for (int i = 0; i < v; i++)
            {
                if (i % 2 != 0)
                    data = data.AddDays(1);

                if (hora >= 24)
                {
                    hora = 1;
                }

                var horaInicial = TimeSpan.Parse($"{hora}:00");

                var qtd = i % 5 == 0 ? 4 : i % 5;

                var medicosParticipantes = medicos.GetRange(qtd, qtd);

                var cirurgia = new Cirurgia(data, horaInicial, 120, medicosParticipantes, usuario);

                var ehValida = cirurgia.VerificarDescansoMedico() && cirurgia.VerificarAgendaMedico(); ;

                if (ehValida == true)
                    cirurgias.Add(cirurgia);

                hora += 6;
            }

            return cirurgias;

        }
        private static void LimparTabelas(EAgendaMedicaDBContext dbContext)
        {
            dbContext.Set<Consulta>().RemoveRange(dbContext.Set<Consulta>());
            dbContext.Set<Cirurgia>().RemoveRange(dbContext.Set<Cirurgia>());
            dbContext.Set<Medico>().RemoveRange(dbContext.Set<Medico>());
            dbContext.SaveChanges();
        }

        private static List<string> nomes = new List<string>() {
                "João Silva",
                "Pedro Oliveira",
                "José Souza",
                "Carlos Machado",
                "Francisco Lima",
                "Antônio Santos",
                "Luiz Pereira",
                "Miguel Araújo",
                "Matheus Nunes",
                "Lucas Ferreira",
                "Maria Almeida",
                "Ana Cardoso",
                "Flávia Martins",
                "Letícia Ribeiro",
                "Isabela Gomes",
                "Carolina Costa",
                "Bruna Andrade",
                "Julia Gonçalves",
                "Heloísa Ramos",
                "Emanuelly Soares"
            };
    }

}
