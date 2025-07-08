using Frontend.Models;

namespace Frontend.Servicos.Interfaces
{
    public interface IAmostraServico
    {
        bool ValidarAmostra(Amostra amostra);
        Task<bool> AdicionarAmostraAsync(Amostra amostra);
    }
}
