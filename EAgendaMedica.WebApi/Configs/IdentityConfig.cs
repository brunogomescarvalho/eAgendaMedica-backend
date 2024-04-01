using Microsoft.AspNetCore.Identity;
using EAgendaMedica.Dominio.ModuloAutenticacao;
using EAgendaMedica.Infra.Compartilhado;
using EAgendaMedica.Servico.ModuloAutenticacao;

namespace EAgendaMedica.WebApi.Configs
{
    public static class IdentityConfig
    {
        public static void ConfigurarIdentity(this IServiceCollection services)
        {
            services.AddIdentity<Usuario, IdentityRole<Guid>>()
               .AddEntityFrameworkStores<EAgendaMedicaDBContext>()
               .AddErrorDescriber<MensagensPortuguesIdentity>()
               .AddDefaultTokenProviders();

            services.AddTransient<UserManager<Usuario>>();
            services.AddTransient<SignInManager<Usuario>>();
            services.AddTransient<ServicoAutenticacao>();
        }

        public class MensagensPortuguesIdentity : IdentityErrorDescriber
        {
            public override IdentityError PasswordTooShort(int length)
            {
                return new IdentityError { Code = nameof(PasswordTooShort), Description = $"Senhas devem conter ao menos {length} caracteres." };
            }
            public override IdentityError PasswordRequiresNonAlphanumeric()
            {
                return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Senhas devem conter ao menos um caracter não alfanumérico." };
            }
            public override IdentityError PasswordRequiresDigit()
            {
                return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Senhas devem conter ao menos um digito ('0'-'9')." };
            }
            public override IdentityError PasswordRequiresLower()
            {
                return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Senhas devem conter ao menos um caracter em caixa baixa ('a'-'z')." };
            }
            public override IdentityError PasswordRequiresUpper()
            {
                return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Senhas devem conter ao menos um caracter em caixa alta ('A'-'Z')." };
            }
            public override IdentityError PasswordMismatch()
            {
                return new IdentityError { Code = nameof(PasswordMismatch), Description = "Senha incorreta." };
            }
            public override IdentityError InvalidToken()
            {
                return new IdentityError { Code = nameof(InvalidToken), Description = "Token inválido." };
            }
            public override IdentityError LoginAlreadyAssociated()
            {
                return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Já existe um usuário com este login." };
            }
            public override IdentityError InvalidUserName(string userName)
            {
                return new IdentityError { Code = nameof(InvalidUserName), Description = $"Login '{userName}' é inválido, pode conter apenas letras ou dígitos." };
            }
            public override IdentityError InvalidEmail(string email)
            {
                return new IdentityError { Code = nameof(InvalidEmail), Description = $"Email '{email}' é inválido." };
            }
            public override IdentityError DuplicateUserName(string userName)
            {
                return new IdentityError { Code = nameof(DuplicateUserName), Description = $"Login '{userName}' já está sendo utilizado." };
            }
            public override IdentityError DuplicateEmail(string email)
            {
                return new IdentityError { Code = nameof(DuplicateEmail), Description = $"Email '{email}' já está sendo utilizado." };
            }
        }
    }
}
