using Backend.Models;
using Backend.Repositorio.Interfaces;
using Backend.Repositorio.Principal;
using Backend.Servicos.Interfaces;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Backend.Servicos.Principal
{
    public class AuthServico : IAuthServico
    {
        private readonly ILogServico _logServico;
        private readonly IUsuarioRepositorio _usuarioRepo;

        public AuthServico(ILogServico logServico, IUsuarioRepositorio usuarioRepo)
        {
            _logServico = logServico;
            _usuarioRepo = usuarioRepo;
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
    }
}
