using Backend.Models;

namespace Backend.Repositorio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario?> ConsultarUsuarioAsync(string usuario);
        Task<bool> AdicionarUsuarioAsync(Usuario usuario);
    }
}
