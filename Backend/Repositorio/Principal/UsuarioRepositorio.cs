using Backend.Data;
using Backend.Models;
using Backend.Repositorio.Interfaces;
using Backend.Servicos.Interfaces;

namespace Backend.Repositorio.Principal
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ILogServico _logServico;
        private readonly SqlContext _sqlContext;

        public UsuarioRepositorio(ILogServico logServico, SqlContext sqlContext)
        {
            _logServico = logServico;
            _sqlContext = sqlContext;
        }

        public async Task<Usuario?> ConsultarUsuarioAsync(string usuario)
        {
            try
            {
                var usuarioEncontrado = await _sqlContext.Usuarios.FindAsync(usuario);

                if (usuarioEncontrado == null)
                    return null;

                return usuarioEncontrado;
            }
            catch (Exception ex) {
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(ConsultarUsuarioAsync)}: {ex.Message}");
                return null!;
            }
        }

        public async Task<bool> AdicionarUsuarioAsync(Usuario usuario)
        {
            try
            {
                await _sqlContext.Usuarios.AddAsync(usuario);

                int mudancas = await _sqlContext.SaveChangesAsync();

                if(mudancas > 0)
                {
                    return true;
                }
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
