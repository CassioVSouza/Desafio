using Backend.Models;

namespace Backend.Repositorio.Interfaces
{
    public interface IAmostraRepositorio
    {
        Task<bool> AdicionarAmostraAsync(Amostra amostra);
        Task<bool> DeletarAmostraAsync(string codigo);
        Task<bool> AtualizarAmostraAsync(Amostra amostra);
        Task<List<Amostra>> ConsultarAmostrasAsync();
        Task<List<Amostra>> ConsultarAmostrasFiltradasAsync(DateTime? Data, string? Status);
        Task<Amostra?> ConsultarAmostraPorCodigoAsync(string codigo);
    }
}
