using Microsoft.IdentityModel.Tokens;
using EAgendaMedica.Dominio.ModuloAutenticacao;
using EAgendaMedica.WebApi.ViewsModels.AutenticacaoVM;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EAgendaMedica.WebApi.Services
{
    public class JwtService
    {
        public TokenViewModel GerarJwt(Usuario usuario)
        {
            var identityClaims = new ClaimsIdentity();

            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, usuario.Email));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.GivenName, usuario.Nome));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SegredoSuperSecreto");
            DateTime dataExpiracao = DateTime.UtcNow.AddHours(8);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "EAgendaMedica",
                Audience = "http://localhost",
                Subject = identityClaims,
                Expires = dataExpiracao,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new TokenViewModel
            {
                Chave = encodedToken,
                DataExpiracao = dataExpiracao,
                UsuarioToken = new UsuarioTokenViewModel
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                }
            };

            return response;
        }
    }
}
