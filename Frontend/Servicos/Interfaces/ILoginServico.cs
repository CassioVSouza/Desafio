using Frontend.Enums;
using Frontend.Models;

namespace Frontend.Servicos.Interfaces
{
    public interface ILoginServico
    {
        Task<ERespostaAPI> FazerLoginAsync(LoginModel login);
    }
}
