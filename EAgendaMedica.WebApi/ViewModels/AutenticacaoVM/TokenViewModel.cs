namespace EAgendaMedica.WebApi.ViewsModels.AutenticacaoVM
{
    public class TokenViewModel
    {
       
            public string Chave { get; set; }

            public UsuarioTokenViewModel UsuarioToken { get; set; }

            public DateTime DataExpiracao { get; set; }
        }
    
}
