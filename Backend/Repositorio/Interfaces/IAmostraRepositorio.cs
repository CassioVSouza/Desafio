using Backend.Models;

namespace Backend.Repositorio.Interfaces
{
    public interface IAmostraRepositorio
    {
        Task<bool> AdicionarAmostra(Amostra amostra);
        Task<bool> DeletarAmostra(int codigo);
        Task<bool> AtualizarAmostra(Amostra amostra);
        Task<List<Amostra>> ConsultarAmostras();
        Task<List<Amostra>> ConsultarAmostrasFiltradas(DateTime Data, string Status);
    }
}
