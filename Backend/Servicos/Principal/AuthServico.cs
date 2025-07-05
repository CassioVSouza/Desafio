using Azure.Core;
using Backend.Models;
using Backend.Repositorio.Interfaces;
using Backend.Repositorio.Principal;
using Backend.Servicos.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Backend.Servicos.Principal
{
    public class AuthServico : IAuthServico
    {
        private readonly ILogServico _logServico;
        private readonly IUsuarioRepositorio _usuarioRepo;
        private readonly JwtConfigs _jwtConfigs;

        public AuthServico(ILogServico logServico, IUsuarioRepositorio usuarioRepo, IOptions<JwtConfigs> configJWT)
        {
            _logServico = logServico;
            _usuarioRepo = usuarioRepo;
            _jwtConfigs = configJWT.Value;
        }

        public Usuario ValidarInformacoes(string usuarioNome, string senha)
        {
            usuarioNome = usuarioNome.Trim();
            senha = senha.Trim();

            bool validacaoUsario = usuarioNome.Length <= 20 && usuarioNome.Length >= 6;
            bool validacaoSenha = senha.Length <= 30 && senha.Length >= 4;

            if(validacaoSenha && validacaoUsario)
            {
                return new Usuario()
                {
                    Senha = senha,
                    User = usuarioNome,
                };
            }

            return null!;
        }

        public async Task<bool> UsuarioNaoExisteAsync(string usuario)
        {
            try
            {
                var usuarioEncontrado = await _usuarioRepo.ConsultarUsuarioAsync(usuario);

                if (usuarioEncontrado == null)
                    return true;

                return false;
            }
            catch (Exception ex) {
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(UsuarioNaoExisteAsync)}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AdicionarUsuarioAsync(Usuario usuario)
        {
            try
            {
                var resposta = await _usuarioRepo.AdicionarUsuarioAsync(usuario);

                if (resposta)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(AdicionarUsuarioAsync)}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ValidarUsuarioAsync(string usuario, string senha)
        {
            try
            {
                var usuarioEncontrado = await _usuarioRepo.ConsultarUsuarioAsync(usuario);

                if (usuarioEncontrado == null)
                    return false;

                if (senha == usuarioEncontrado.Senha)
                    return true;

                return false;

            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(ValidarUsuarioAsync)}: {ex.Message}");
                return false;
            }
        }

        public string? RetornarTokenDeAcessoAsync(string usuario)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, usuario),
                    new Claim(ClaimTypes.Role, "User")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigs.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwtConfigs.Issuer,
                    audience: _jwtConfigs.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(_jwtConfigs.ExpireDays),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(ValidarUsuarioAsync)}: {ex.Message}");
                return null!;
            }
        }
    }
}
