using Frontend.Enums;
using Frontend.Models;

namespace Frontend.Servicos.Interfaces
{
    public interface IAmostraServico
    {
        bool ValidarAmostra(Amostra amostra);
        Task<bool> AdicionarAmostraAsync(Amostra amostra);
        Task<List<Amostra>?> ConsultarAmostrasAsync();
        Task<(ERespostaAPI? respostaErro, Amostra? amostra)> ConsultarAmostraPorCodigoAsync(string codigo);
        Task<ERespostaAPI> EditarAmostraAsync(Amostra amostra);
        Task<bool> RemoverAmostraAsync(string codigo);
        Task<List<Amostra>?> ConsultarAmostrasFiltradasAsync(Filtro filtro);
    }
}
