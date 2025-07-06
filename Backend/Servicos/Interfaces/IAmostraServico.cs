using Backend.DTOs;
using Backend.Models;

namespace Backend.Servicos.Interfaces
{
    public interface IAmostraServico
    {
        Task<bool> AdicionarAmostraAsync(Amostra amostra);
        Task<bool> AtualizarAmostraAsync(Amostra amostra);
        Task<bool> ExcluirAmostraAsync(string codigo);
        Task<Amostra?> ConsultarAmostraPorCodigoAsync(string codigo);
        Task<List<Amostra>> ConsultarAmostrasAsync();
        Task<List<Amostra>> ConsultarAmostrasFiltradasAsync(DateTime? date, string? status);
        Amostra? ValidarInformacoes(AmostraDTO amostra);
    }
}
