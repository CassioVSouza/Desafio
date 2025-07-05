using Backend.Models;

namespace Backend.Servicos.Interfaces
{
    public interface IAuthServico
    {
        Task<bool> AdicionarUsuarioAsync(Usuario usuario);
        Task<bool> UsuarioNaoExisteAsync(string usuario);
        Usuario ValidarInformacoes(string usuarioNome, string senha);
        string? RetornarTokenDeAcessoAsync(string usuario);
        Task<bool> ValidarUsuarioAsync(string usuario, string senha);
    }
}
